using System.Text;

namespace Automata.Daemon.Helpers;

internal class ConsoleTableBuilder
{
    public enum ColumnAlignment
    {
        Left,
        Center,
        Right
    }

    private readonly List<ConsoleTableColumn> _columns;
    private readonly List<string?[]> _rows;

    public ConsoleTableBuilder()
    {
        _rows = new List<string?[]>();
        _columns = new List<ConsoleTableColumn>();
    }

    public ConsoleTableBuilder AddColumn(string header, int? width = null, ColumnAlignment alignment = ColumnAlignment.Left)
    {
        _columns.Add(new ConsoleTableColumn(header, width, alignment));
        return this;
    }

    public ConsoleTableBuilder AddRow(params string?[] values)
    {
        _rows.Add(values);
        return this;
    }

    public void Write(bool skipHeader = false, bool useBorders = false)
    {
        StringBuilder sb = new();

        if (!skipHeader)
        {
            WriteHeaders(sb, useBorders);
        }

        for (int i = 0; i < _rows.Count; i++)
        {
            WriteRow(sb, _rows[i], useBorders);
        }

        Console.WriteLine(sb);
    }

    private void WriteHeaders(StringBuilder sb, bool useBorders)
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            var column = _columns[i];
            int columnWidth = column.Width is null ? GetMaxColumnWidth(i) : column.Width.Value;

            sb.Append(Align(column.Header, columnWidth, column.Alignment));
            sb.Append(useBorders ? " | " : " ");
        }

        sb.AppendLine();

        if (useBorders)
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                var column = _columns[i];
                int columnWidth = column.Width is null ? GetMaxColumnWidth(i) : column.Width.Value;

                sb.Append(new string('-', columnWidth));
                sb.Append("-+-");
            }

            sb.AppendLine();
        }
    }

    private void WriteRow(StringBuilder sb, string?[] row, bool useBorders)
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            var column = _columns[i];
            int columnWidth = column.Width is null ? GetMaxColumnWidth(i) : column.Width.Value;

            sb.Append(Align(row[i] ?? string.Empty, columnWidth, column.Alignment));
            sb.Append(useBorders ? " | " : " ");
        }

        sb.AppendLine();
    }

    private string Align(string text, int width, ColumnAlignment alignment)
    {
        if (alignment == ColumnAlignment.Left)
        {
            return text.Length < width ? text.PadRight(width, ' ') : text.Substring(0, width);
        }
        else if (alignment == ColumnAlignment.Right)
        {
            return text.Length < width ? text.PadLeft(width, ' ') : text.Substring(0, width);
        }
        else
        {
            return text.Length < width ? text.PadRight(width - (width - text.Length) / 2).PadLeft(width) : text.Substring(0, width);
        }
    }

    private string[] GetColumnValues(int columnIndex)
    {
        var values = new List<string>();
        foreach (var row in _rows)
        {
            values.Add(columnIndex < row.Length ? row[columnIndex] : string.Empty);
        }

        return values.ToArray();
    }

    private int GetMaxColumnWidth(int columnIndex)
    {
        var columnValues = GetColumnValues(columnIndex);
        var valuesColumnWidth = columnValues.Any() ? columnValues.Max(v => v.Length) : 0;
        return Math.Max(valuesColumnWidth, _columns[columnIndex].Header.Length);
    }

    private sealed class ConsoleTableColumn
    {
        public ConsoleTableColumn(string header, int? width, ColumnAlignment alignment)
        {
            Header = header;
            Width = width;
            Alignment = alignment;
        }

        public string Header { get; set; }
        public int? Width { get; set; }
        public ColumnAlignment Alignment { get; set; }
    }
}
