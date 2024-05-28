using System;

namespace LangLang.Domain.Model
{
    public class ExamGrade
    {
        private readonly uint _readingScore;
        public uint ReadingScore
        {
            get => _readingScore;
            init
            {
                if (value > Constants.MaxReadingScore)
                    throw new ArgumentException("Invalid reading score range");
                _readingScore = value;
            }
        }

        private readonly uint _writingScore;
        public uint WritingScore
        {
            get => _writingScore;
            init
            {
                if (value > Constants.MaxWritingScore)
                    throw new ArgumentException("Invalid writing score range");
                _writingScore = value;
            }
        }

        private readonly uint _listeningScore;
        public uint ListeningScore
        {
            get => _listeningScore;
            init
            {
                if (value > Constants.MaxListeningScore)
                    throw new ArgumentException("Invalid listening score range");
                _listeningScore = value;
            }
        }

        private readonly uint _speakingScore;
        public uint SpeakingScore
        {
            get => _speakingScore;
            init
            {
                if (value > Constants.MaxSpeakingScore)
                    throw new ArgumentException("Invalid speaking score range");
                _speakingScore = value;
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

        public bool IsPassing()
        {
            if (ReadingScore < CeilDivide(Constants.MaxReadingScore, 2))
                return false;
            if (WritingScore < CeilDivide(Constants.MaxWritingScore, 2))
                return false;
            if (ListeningScore < CeilDivide(Constants.MaxListeningScore, 2))
                return false;
            if (SpeakingScore < CeilDivide(Constants.MaxSpeakingScore, 2))
                return false;
            var sum = ReadingScore + WritingScore + ListeningScore + SpeakingScore;
            return sum > Constants.MinPassingScore;
        }
        
        private static long CeilDivide(uint a, int b)
        {
            return (a + b - 1) / b;
        }
    }
}