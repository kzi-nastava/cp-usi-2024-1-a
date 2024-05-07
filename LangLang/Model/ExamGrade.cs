using System;
using Consts;

namespace LangLang.Model
{
    public class ExamGrade
    {
        private uint _readingScore;
        public uint ReadingScore
        {
            get => _readingScore;
            set
            {
                if (value > Constants.MaxReadingScore)
                    throw new ArgumentException("Invalid reading score range");
                _readingScore = value;
            }
        }

        private uint writingScore;
        public uint WritingScore
        {
            get => writingScore;
            set
            {
                if (value > Constants.MaxWritingScore)
                    throw new ArgumentException("Invalid writing score range");
                writingScore = value;
            }
        }

        private uint listeningScore;
        public uint ListeningScore
        {
            get => listeningScore;
            set
            {
                if (value > Constants.MaxListeningScore)
                    throw new ArgumentException("Invalid listening score range");
                listeningScore = value;
            }
        }

        private uint speakingScore;
        public uint SpeakingScore
        {
            get => speakingScore;
            set
            {
                if (value > Constants.MaxSpeakingScore)
                    throw new ArgumentException("Invalid speaking score range");
                speakingScore = value;
            }
        }

        public ExamGrade()
        {
        }

        public ExamGrade(uint readingScore, uint writingScore, uint listeningScore, uint speakingScore)
        {
            ReadingScore = readingScore;
            WritingScore = writingScore;
            ListeningScore = listeningScore;
            SpeakingScore = speakingScore;
        }
    }
}