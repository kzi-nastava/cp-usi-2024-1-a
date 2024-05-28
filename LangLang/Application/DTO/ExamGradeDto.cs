namespace LangLang.Application.DTO;

public class ExamGradeDto
{
    public uint Reading { get; }
    public uint Writing { get; }
    public uint Listening { get; }
    public uint Speaking { get; }

    public ExamGradeDto(uint reading, uint writing, uint listening, uint speaking)
    {
        Reading = reading;
        Writing = writing;
        Listening = listening;
        Speaking = speaking;
    }
}