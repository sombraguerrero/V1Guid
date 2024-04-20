using System.Runtime.InteropServices;
using System.Text;
using System.Data.SqlClient;

[DllImport("rpcrt4.dll", SetLastError = true)] //DLL still present as of Windows 11 - both architectures
static extern int UuidCreateSequential(out Guid guid); //Function names are case-sensitive per the output of dumpbin /exports 

const int RPC_S_OK = 0;
int num;
Guid g;
if (args.Length == 1 && int.TryParse(args[0], out num))
{
    //A BIN2 coallation on the database would seem to make this more optimal for case-sensitivity and the use of the binary
    //Guid as a primary key
    using (SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("tsql_test", EnvironmentVariableTarget.User)))
    {
        try
        {
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                SqlCommand command = new SqlCommand("INSERT INTO [dbo].[myguids] (ordered) values (@someGuid)", conn, trans);
                command.Parameters.Add("someguid", System.Data.SqlDbType.VarBinary);
                for (int i = 0; i < num; i++)
                {
                    try
                    {
                        if (UuidCreateSequential(out g) == RPC_S_OK)
                        {
                            string gString = g.ToString().ToUpper();
                            string gSubString = gString.Substring(14, 4) + gString.Substring(9, 4) + gString.Substring(0, 8) + gString.Substring(19, 4) + gString.Substring(24);
                            Console.WriteLine(gSubString);
                            command.Parameters["someguid"].Value = Encoding.Default.GetBytes(gSubString);
                            command.ExecuteNonQuery();
                        }
                        else
                            Console.WriteLine("Creation of this GUID failed!");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SqlException inside the loop: {0}", ex.Message);
                        trans.Rollback();
                    }
                }
                trans.Commit();
            }
        }
        catch (SqlException ex)
        {
            Console.Error.WriteLine("SqlException outside the loop: {0}", ex.Message);
        }
    }
}
else if (args.Length == 1 && args[0].ToLower().Equals("--stdin"))
{
    Console.Write("Number of Guids: ");
    if (int.TryParse(Console.ReadLine(), out num))
    {
        //A BIN2 coallation on the database would seem to make this more optimal for case-sensitivity and the use of the binary
        //Guid as a primary key
        using (SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("tsql_test", EnvironmentVariableTarget.User)))
        {
            try
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [dbo].[myguids] (ordered) values (@someGuid)", conn, trans);
                    command.Parameters.Add("someguid", System.Data.SqlDbType.VarBinary);
                    for (int i = 0; i < num; i++)
                    {
                        try
                        {
                            if (UuidCreateSequential(out g) == RPC_S_OK)
                            {
                                string gString = g.ToString().ToUpper();
                                string gSubString = gString.Substring(14, 4) + gString.Substring(9, 4) + gString.Substring(0, 8) + gString.Substring(19, 4) + gString.Substring(24);
                                Console.WriteLine(gSubString);
                                command.Parameters["someguid"].Value = Encoding.Default.GetBytes(gSubString);
                                command.ExecuteNonQuery();
                            }
                            else
                                Console.WriteLine("Creation of this GUID failed!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            trans.Rollback();
                        }
                    }
                    trans.Commit();
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}

else if (UuidCreateSequential(out g) == RPC_S_OK)
{
    string gString = g.ToString().ToUpper();
    string gSubString = gString.Substring(14, 4) + gString.Substring(9, 4) + gString.Substring(0, 8) + gString.Substring(19, 4) + gString.Substring(24);
    byte[] gSubStringBytes = Encoding.Default.GetBytes(gSubString);
    string hexString = @"0x" + Convert.ToHexString(gSubStringBytes);
    Console.WriteLine("Original: {0}\r\nSubstringed: {1}\r\nHex: {2}\r\nVia BitConverter: {3}\r\nByte array length: {4}", gString, gSubString, hexString, @"0x" + BitConverter.ToString(gSubStringBytes).Replace("-", string.Empty), gSubStringBytes.Length);

    //A BIN2 coallation on the database would seem to make this more optimal for case-sensitivity and the use of the binary
    //Guid as a primary key
    using (SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("tsql_test", EnvironmentVariableTarget.User)))
    {
        try
        {
            conn.Open();
            SqlCommand command = new SqlCommand("INSERT INTO [dbo].[myguids] (ordered) values (@someGuid)", conn);
            command.Parameters.AddWithValue("someguid", gSubStringBytes);
            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
else
{
    Console.WriteLine("No quantity provided and single GUID generation failed!");
}
Console.Write("Press any key to continue...");
_ = Console.ReadKey();