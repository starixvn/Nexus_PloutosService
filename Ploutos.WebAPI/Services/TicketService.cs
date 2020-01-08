using Ploutos.WebAPI.Models;
using Ploutos.WebAPI.Repositories;
using Ploutos.WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.WebAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;

        public TicketService() : this(null)
        {
        }

        public TicketService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository ?? new TicketRepository();
        }

        public async Task<TicketResponse> InsertTickets(List<BetTicket> betTickets)
        {
            var ticketData = await ticketRepository.GetDuplicateTicketData(betTickets);
            var ticketSettings = await ticketRepository.GetTicketSettings();

            var insertTickets = new List<BetTicket>();
            var errors = new List<TicketResponseData>();
            foreach(var ticket in betTickets)
            {
                var error = ValidateTicket(ticket, ticketSettings, ticketData);
                if (string.IsNullOrEmpty(error))
                {
                    UpdateTicketData(ticket);
                    insertTickets.Add(ticket);
                }
                else
                {
                    errors.Add(new TicketResponseData { TransactionNo = ticket.TransactionNo, ErroMessage = error });
                }
            }

            var result = await ticketRepository.InserBetTickets(insertTickets);

            if (!result.IsSuccessful)
            {
                foreach(var ticket in insertTickets)
                {
                    errors.Add(new TicketResponseData { TransactionNo = ticket.TransactionNo, ErroMessage = result.Message });
                }
            }

            return new TicketResponse { Data = errors };
        }

        private static void UpdateTicketData(BetTicket ticket)
        {
            ticket.CreatedById = "9cabbc12-e688-47ca-ae5d-2c8ebdc5cd38";
            ticket.ModifiedById = "9cabbc12-e688-47ca-ae5d-2c8ebdc5cd38";
            ticket.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ticket.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ticket.TransactionDate = DateTime.Parse(ticket.TransactionDate).ToString("yyyy-MM-dd HH:mm:ss");
            ticket.SettlementDate = DateTime.Parse(ticket.SettlementDate).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private static string ValidateTicket(BetTicket ticket, TicketSettingData settings, List<string> existingTransactionNoList)
        {
            var result = string.Empty;
            if (!settings.GamePlatformCodeList.Contains(ticket.GamePlatformCode))
            {
                result = "GamePlatformCode error";
            }
            else if (!settings.GamePlatformProductCodeList.Contains(ticket.GameId))
            {
                result = "GameId error";
            }
            else if (!settings.UserAccountNoList.Contains(ticket.PlayerCode))
            {
                result = "PlayerCode error";
            }
            else if (existingTransactionNoList.Contains(ticket.TransactionNo))
             {
                result = "TransactionNo error";
            }
            else
            {
                // Do nothing
            }

            return result;
        }        
    }
}