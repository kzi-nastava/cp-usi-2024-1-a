using System;
using Consts;

namespace LangLang.Model
{
    public class ExamGrade
    {
        private int _readingScore;
        public int ReadingScore
        {
            get => _readingScore;
            set
            {
                if (value < 0 || value > Constants.MaxReadingScore)
                    throw new ArgumentException("Invalid reading score range");
                _readingScore = value;
            }
        }

        private int writingScore;
        public int WritingScore
        {
            get => writingScore;
            set
            {
                if (value < 0 || value > Constants.MaxWritingScore)
                    throw new ArgumentException("Invalid writing score range");
                writingScore = value;
            }
        }

        private int listeningScore;
        public int ListeningScore
        {
            get => listeningScore;
            set
            {
                if (value < 0 || value > Constants.MaxListeningScore)
                    throw new ArgumentException("Invalid listening score range");
                listeningScore = value;
            }
        }

        private int speakingScore;
        public int SpeakingScore
        {
            get => speakingScore;
            set
            {
                if (value < 0 || value > Constants.MaxSpeakingScore)
                    throw new ArgumentException("Invalid speaking score range");
                speakingScore = value;
            }
        }

        public ExamGrade()
        {
        }

        public ExamGrade(int readingScore, int writingScore, int listeningScore, int speakingScore)
        {
            ReadingScore = readingScore;
            WritingScore = writingScore;
            ListeningScore = listeningScore;
            SpeakingScore = speakingScore;
        }
    }
}