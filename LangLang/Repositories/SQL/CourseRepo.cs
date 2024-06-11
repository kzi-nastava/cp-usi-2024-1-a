using LangLang.Domain.Model;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace LangLang.Repositories.SQL
{
	public class CourseRepo
	{
		
        private string connectionString = "Host=localhost;Port=5433;Username=postgres;Password=123;Database=langlang;";
        public CourseRepo()
        {
            // Create the courses table if it doesn't exist
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS courses (
                                id VARCHAR(100) PRIMARY KEY,
                                name VARCHAR(100),
                                language_name VARCHAR(100),
                                language_code VARCHAR(10),
                                level VARCHAR(50),
                                duration INT,
                                start TIMESTAMP,
                                online BOOLEAN,
                                max_students INT,
                                num_students INT,
                                state VARCHAR(50),
                                tutor_id VARCHAR(100),
                                is_created_by_tutor BOOLEAN,
                                CONSTRAINT courses_id_unique UNIQUE (id)
                            );";
                    cmd.ExecuteNonQuery();
                }
                // Create the course_schedule table if it doesn't exist
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS course_schedule (
                            id SERIAL PRIMARY KEY,
                            course_id VARCHAR(100),
                            work_day VARCHAR(50),
                            start_time TIME,
                            duration INT,
                            CONSTRAINT course_schedule_unique UNIQUE (course_id, work_day), -- Add unique constraint
                            FOREIGN KEY (course_id) REFERENCES courses(id)
                        );";
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void InsertCourse(Course course)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                INSERT INTO courses (id, name, language_name, language_code, level, duration, start, online, max_students, num_students, state, tutor_id, is_created_by_tutor) 
                VALUES (@Id, @Name, @LanguageName, @LanguageCode, @Level, @Duration, @Start, @Online, @MaxStudents, @NumStudents, @State, @TutorId, @IsCreatedByTutor)
                ON CONFLICT (id) DO UPDATE
                SET name = EXCLUDED.name, 
                    language_name = EXCLUDED.language_name,
                    language_code = EXCLUDED.language_code,
                    level = EXCLUDED.level,
                    duration = EXCLUDED.duration,
                    start = EXCLUDED.start,
                    online = EXCLUDED.online,
                    max_students = EXCLUDED.max_students,
                    num_students = EXCLUDED.num_students,
                    state = EXCLUDED.state,
                    tutor_id = EXCLUDED.tutor_id,
                    is_created_by_tutor = EXCLUDED.is_created_by_tutor";

                    cmd.Parameters.AddWithValue("Id", course.Id); // Use course.Id as course_id
                    cmd.Parameters.AddWithValue("Name", course.Name);
                    cmd.Parameters.AddWithValue("LanguageName", course.Language.Name);
                    cmd.Parameters.AddWithValue("LanguageCode", course.Language.Code);
                    cmd.Parameters.AddWithValue("Level", course.Level.ToString());
                    cmd.Parameters.AddWithValue("Duration", course.Duration);
                    cmd.Parameters.AddWithValue("Start", course.Start);
                    cmd.Parameters.AddWithValue("Online", course.Online);
                    cmd.Parameters.AddWithValue("MaxStudents", course.MaxStudents);
                    cmd.Parameters.AddWithValue("NumStudents", course.NumStudents);
                    cmd.Parameters.AddWithValue("State", course.State.ToString());
                    cmd.Parameters.AddWithValue("TutorId", course.TutorId);
                    cmd.Parameters.AddWithValue("IsCreatedByTutor", course.IsCreatedByTutor);

                    cmd.ExecuteNonQuery(); // Execute the insert query

                    // Insert or update schedule into the same table
                    InsertOrUpdateSchedule(course.Id, course.Schedule);
                }
            }
        }


        private void InsertOrUpdateSchedule(string courseId, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule)
        {
            foreach (var kvp in schedule)
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                    INSERT INTO course_schedule (course_id, work_day, start_time, duration) 
                    VALUES (@CourseId, @WorkDay, @StartTime, @Duration)
                    ON CONFLICT (course_id, work_day) DO UPDATE
                    SET start_time = EXCLUDED.start_time,
                        duration = EXCLUDED.duration";

                        cmd.Parameters.AddWithValue("CourseId", courseId); // courseId is already a string
                        cmd.Parameters.AddWithValue("WorkDay", kvp.Key.ToString());

                        // Convert TimeOnly to TimeSpan
                        var startTime = TimeSpan.FromHours(kvp.Value.Item1.Hour) +
                                        TimeSpan.FromMinutes(kvp.Value.Item1.Minute) +
                                        TimeSpan.FromSeconds(kvp.Value.Item1.Second);
                        cmd.Parameters.AddWithValue("StartTime", NpgsqlDbType.Time, startTime);

                        cmd.Parameters.AddWithValue("Duration", kvp.Value.Item2);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }



        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM courses", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Course course = new Course
                            {
                                Id = reader["id"].ToString()!,
                                Name = reader["name"].ToString()!,
                                Language = new Language(reader["language_name"].ToString()!, reader["language_code"].ToString()!),
                                Level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), reader["level"].ToString()!),
                                Duration = Convert.ToInt32(reader["duration"]),
                                Start = Convert.ToDateTime(reader["start"]),
                                Online = Convert.ToBoolean(reader["online"]),
                                MaxStudents = Convert.ToInt32(reader["max_students"]),
                                NumStudents = Convert.ToInt32(reader["num_students"]),
                                State = (Course.CourseState)Enum.Parse(typeof(Course.CourseState), reader["state"].ToString()),
                                TutorId = reader["tutor_id"].ToString(),
                                IsCreatedByTutor = Convert.ToBoolean(reader["is_created_by_tutor"]),
                                Schedule = GetCourseSchedule(reader["id"].ToString()!) // Load schedule from the same table
                            };
                            courses.Add(course);
                        }
                    }
                }
            }
            return courses;
        }

        // Helper method to retrieve schedule from the same table
        private Dictionary<WorkDay, Tuple<TimeOnly, int>> GetCourseSchedule(string courseId)
        {
            var schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM course_schedule WHERE course_id = @CourseId", conn))
                {
                    // Convert courseId to integer
                    cmd.Parameters.AddWithValue("CourseId", courseId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var workDay = Enum.Parse<WorkDay>(reader["work_day"].ToString());
                            var startTime = TimeOnly.Parse(reader["start_time"].ToString());
                            var duration = Convert.ToInt32(reader["duration"]);
                            schedule.Add(workDay, Tuple.Create(startTime, duration));
                        }
                    }
                }
            }
            return schedule;
        }

        // Implement UpdateCourse and DeleteCourse methods if needed

    }

}
