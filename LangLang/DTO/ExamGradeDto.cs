namespace LangLang.DTO;

public class ExamGradeDto
{
    public int Reading { get; }
    public int Writing { get; }
    public int Listening { get; }
    public int Speaking { get; }

    public ExamGradeDto(int reading, int writing, int listening, int speaking)
    {
        Reading = reading;
        Writing = writing;
        Listening = listening;
        Speaking = speaking;
    }
}