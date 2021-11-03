using System;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace todoList
{
    class ConnectToBD
    {
        // private List<TodoItem> tasks = new List<TodoItem>();

        public async Task<List<TodoItem>> ReadDB(NpgsqlConnection conn)
        {

            List<TodoItem> tasks = new List<TodoItem>();


            await using (var cmd = new NpgsqlCommand("Select \"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\" from todo", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    tasks.Add(new TodoItem(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2), reader.GetBoolean(3), reader.GetInt32(4)));

                }
            return tasks;
        }

        public async Task<bool> Create(NpgsqlConnection conn, string Title, string Desc, DateTime DueDate, bool Done, int List_Group)
        {
            try
            {
                await using (var cmd = new NpgsqlCommand("INSERT INTO todo (\"Title\", \"Description\", \"DueDate\", \"Done\", \"List_Group\") VALUES (@Title, @Description, @DueDate, @Done, @List_Group)", conn))
                {
                    cmd.Parameters.AddWithValue("Title", Title);
                    cmd.Parameters.AddWithValue("Description", Desc);
                    cmd.Parameters.AddWithValue("DueDate", DueDate);
                    cmd.Parameters.AddWithValue("Done", Done);
                    cmd.Parameters.AddWithValue("List_Group", List_Group);
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(NpgsqlConnection conn, int id, TaskColumn taskColumn)
        {
                await using (var cmd = new NpgsqlCommand("UPDATE todo SET \"Title\" = @Title, \"Description\" = @Description, \"DueDate\" = @Duedate, \"Done\" = @Done, \"List_Group\" = @List_Group WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("Title", taskColumn.Title);
                    cmd.Parameters.AddWithValue("Description", taskColumn.Desc);
                    cmd.Parameters.AddWithValue("Duedate", taskColumn.DueDate);
                    cmd.Parameters.AddWithValue("Done", taskColumn.Done);
                    cmd.Parameters.AddWithValue("List_Group", taskColumn.List_Group);
                    cmd.Parameters.AddWithValue("id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
        }

        public async Task<bool> Delete(NpgsqlConnection conn, int idDelete)
        {
            try
            {
                await using (var cmd = new NpgsqlCommand("DELETE FROM todo WHERE \"id\"=@id;", conn))
                {
                    cmd.Parameters.AddWithValue("id", idDelete);
                    await cmd.ExecuteNonQueryAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}