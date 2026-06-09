using Microsoft.Data.SqlClient;

var sqlFilePath = "SELECT.SQL";
var CsvFilePath = DateTime.Now.ToString("yyMMddHHmmss") + "OUTPUT.CSV";
var connectionString =
    "Server=SERVER;Database=DATABASE;User Id=USER;Password=PASSWORD;TrustServerCertificate=True;";

try
{
    if (!File.Exists(sqlFilePath) || new FileInfo(sqlFilePath).Length == 0)
    {
        Console.WriteLine($"SQL file not found: {sqlFilePath}");
        return;
    }

    string sql = File.ReadAllText(sqlFilePath);

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            command.CommandTimeout = 300; // seconds

            if (sql.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    using StreamWriter writer = new StreamWriter(CsvFilePath);
                    int fieldCount = reader.FieldCount;

                    // Header:
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + "\t");
                        if (i > 0) writer.Write(",");
                        writer.Write(reader.GetName(i));
                    }
                    writer.WriteLine();
                    Console.WriteLine();

                    // Rows:
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader[i].ToString() + "\t");
                            if (i > 0) writer.Write(",");
                            writer.Write($"\"{reader[i]?.ToString()?.Replace("\"", "\"\"")}\"");
                        }
                        writer.WriteLine();
                        Console.WriteLine();
                    }
                }
            }
            else if (sql.TrimStart().StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) ||
                     sql.TrimStart().StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) ||
                     sql.TrimStart().StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Are you sure you want to execute next command (y\\N)?\r\n");
                Console.WriteLine(sql);
                Console.WriteLine();

                if (string.Equals(Console.ReadLine() ?? "N", "Y", StringComparison.OrdinalIgnoreCase))
                {
                    int affectedRows = command.ExecuteNonQuery();
                    Console.WriteLine($"SQL executed successfully.");
                    Console.WriteLine($"Affected rows: {affectedRows}");
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error executing SQL:");
    Console.WriteLine(ex.Message);
}
