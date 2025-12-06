using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.Entities;
using LearnX_Data.Entities.EF;
using LearnX_ModelView.Catalog.PayMent;

namespace LearnX_Application.Comman.PayMent
{
    public class PayMentService : EntityBaseRepository<Payment> , IPayMentService
    {
        private readonly LearnXPayMentDbContext _contextData;
        public PayMentService(LearnXPayMentDbContext context): base(context)
        {
            _context = context;
        }
        public async Task<PaymentCreatedResponse> CreateAsync(CreatePaymentRequest req)
        {
            throw new NotImplementedException();
        }
        public async Task HandleMomoNotifyAsync(MomoNotifyDto dto)
        {
            throw new NotImplementedException();
        }
        public async Task<Payment?> GetByOrderCodeAsync(string orderCode)
        {
            throw new NotImplementedException();
        }
    }
}