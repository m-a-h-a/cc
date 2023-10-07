using Microsoft.AspNetCore.Mvc;
using QualifiApi.Interfaces;
using QualifiApi.Models;
using System.Net;

namespace QualifiApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CardController : ControllerBase
    {
        //bring logger in (although unused in this example) in order to log errors
        private readonly ILogger<CardController> _logger;
        readonly iCreditCards _creditCardsRepo;

        public CardController(iCreditCards creditCardRepo, ILogger<CardController> logger)
        {
            _logger = logger;
            _creditCardsRepo = creditCardRepo;
        }

        [HttpPost("prequalification")]
        public async Task<List<CreditCardsModel>> Post(string name, string address, string dateOfBirth, string salary, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            int intSalary = 0;
            
            //check we have everything we require (assuming everything is required)
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(dateOfBirth) || string.IsNullOrEmpty(salary))
            {
                response.Content = new StringContent("One or more of: Name, Address, Date of Birth or Salary was empty.");
                throw new System.Web.Http.HttpResponseException(response);
            }

            //check we can parse the salary and it doesn't have any characters like £ or ,
            try
            {
                intSalary = Int32.Parse(salary);
            }
            catch (FormatException)
            {
                response.Content = new StringContent("Please ensure Salary contains numbers only.");
                throw new System.Web.Http.HttpResponseException(response);
            }

            return await _creditCardsRepo.GetEligbleCreditCards(intSalary, cancellationToken);
        }
    }
}