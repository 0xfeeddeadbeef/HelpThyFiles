using FileHelpers;
using System;
using static System.Console;

internal static class Program
{
    private static void Main()
    {
        var engine = new MultiRecordEngine(typeof(Record00), typeof(Record01))
        {
            RecordSelector = new RecordTypeSelector((_, str) =>
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return null;
                }

                if (str.StartsWith("00", StringComparison.Ordinal))
                {
                    return typeof(Record00);
                }

                if (str.StartsWith("01", StringComparison.Ordinal))
                {
                    return typeof(Record01);
                }

                return null;
            })
        };

        var res = engine.ReadFile("Input.txt");

        foreach (var rec in res)
        {
            Write("new "); Write(rec.GetType().Name); Write("( "); Write(rec.ToString()); WriteLine(" );");
        }
    }
}

[FixedLengthRecord(FixedMode.ExactLength)]
[ConditionalRecord(RecordCondition.IncludeIfBegins, "00")]  // FAIL: Can't use with MultiRecordEngine
[IgnoreEmptyLines]
public sealed class Record00
{
    [FieldFixedLength(2)]
    [FieldAlign(AlignMode.Right, '0')]
    public string RecordType
    {
        get; set;
    }

    [FieldFixedLength(8)]
    [FieldConverter(ConverterKind.Int32)]
    [FieldAlign(AlignMode.Right, '0')]
    [FieldNullValue((object)0)]
    public int SequenceNumber
    {
        get; set;
    }

    [FieldFixedLength(150)]
    [FieldTrim(TrimMode.Both)]
    [FieldAlign(AlignMode.Right, ' ')]
    public string Description
    {
        get; set;
    }

    public override string ToString()
    {
        return $"\'{this.RecordType}\', #{this.SequenceNumber}, \'{this.Description}\'";
    }
}

[FixedLengthRecord(FixedMode.ExactLength)]
[ConditionalRecord(RecordCondition.IncludeIfBegins, "01")]  // FAIL: Can't use with MultiRecordEngine
[IgnoreEmptyLines]
public sealed class Record01
{
    [FieldFixedLength(2)]
    [FieldAlign(AlignMode.Right, '0')]
    public string RecordType
    {
        get; set;
    }

    [FieldFixedLength(8)]
    [FieldConverter(ConverterKind.Int32)]
    [FieldAlign(AlignMode.Right, '0')]
    [FieldNullValue((object)0)]
    public int SequenceNumber
    {
        get; set;
    }

    [FieldFixedLength(8)]
    [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
    public DateTime DateAndTime
    {
        get; set;
    }

    public override string ToString()
    {
        return $"\'{this.RecordType}\', #{this.SequenceNumber}, `{this.DateAndTime.ToString("yyyy-MM-dd")}`";
    }
}
