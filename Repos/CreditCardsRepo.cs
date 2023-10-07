using Azure;
using Microsoft.Data.SqlClient;
using QualifiApi.Interfaces;
using QualifiApi.Models;
using System.Net;

namespace QualifiApi.Repos
{
    public class CreditCardsRepo: iCreditCards
    {
        public async Task<List<CreditCardsModel>> GetEligbleCreditCards(int salary, CancellationToken cancellationToken)
        {
            List<CreditCardsModel> eligbleCreditCards = new List<CreditCardsModel>();

            /*
             * The connection string should not be in this file. If using Azure or similar, then using Key Vault or Active Directory would be a better solution.
             * There should be some security in place between whatever is accessing the API and the API, bearer tokens for example too. 
             */
            using (var sqlConn = new SqlConnection( /* YOUR OWN SQL CONNECTION HERE */ ))
            {
                using (var cmd = new SqlCommand()
                {
                    CommandText = "SELECT * FROM dbo.CreditCards WHERE @salary >= MinSalary",
                    CommandType = System.Data.CommandType.Text,
                    Connection = sqlConn
                })
                {
                    cmd.Parameters.Add("@salary", System.Data.SqlDbType.Int).Value = salary;

                    try
                    {
                        await sqlConn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    /*
                                     * You may not need to send everything back here, depends what the front end requires, id and minsalary may be surplus to requirements for example
                                     */
                                    eligbleCreditCards.Add(new CreditCardsModel(
                                        reader.GetInt32(reader.GetOrdinal("Id")),
                                        reader.GetString(reader.GetOrdinal("CardName")),
                                        reader.GetString(reader.GetOrdinal("CardType")),
                                        reader.GetDecimal(reader.GetOrdinal("APR")),
                                        reader.GetInt32(reader.GetOrdinal("MinSalary"))
                                        ));
                                }
                                reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Content = new StringContent("There was an error getting the results.");
                        throw new System.Web.Http.HttpResponseException(response);
                    }
                    finally
                    { 
                        sqlConn.Close();
                    }

                    return eligbleCreditCards;
                }
            }
        }
    }
}
