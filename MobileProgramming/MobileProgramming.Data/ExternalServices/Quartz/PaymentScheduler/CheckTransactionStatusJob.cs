using MediatR;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using Quartz;

namespace MobileProgramming.Data.ExternalServices.Quartz.PaymentScheduler
{
    public class CheckTransactionStatusJob : IJob
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediator _mediator;
        private readonly IRedisCaching _caching;

        public CheckTransactionStatusJob(ISchedulerFactory scheduler, IOrderRepository orderRepository, IPaymentRepository paymentRepository, IZaloPayService zaloPayService, IRedisCaching caching)
        {
            _scheduler = scheduler;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _zaloPayService = zaloPayService;
            _caching = caching;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;
            List<string> cacheKeys = new List<string>();

            cacheKeys = await _caching.SearchKeysAsync("payment");
            //if (cacheKeys == null)
            //{
            //    var processingTransaction = await _transactionRepository.getProcessingTransaction();
            //    cacheKeys = (List<string>)processingTransaction;
            //}


            var tasks = cacheKeys.Select(async key =>
            {
                // Get transactionId from cache

                string? transactionId = null;

                transactionId = await _caching.HashGetSpecificKeyAsync(key, "transactionId");

                //if (string.IsNullOrEmpty(transactionId))
                //{
                //    var existInDb = await _paymentRepository.GetById(key);
                //    if (existInDb == null || existInDb.Order.OrderStatus != "Pending")
                //        return;

                //}

                var queryOrder = await _mediator.Send(new QueryOrder(transactionId));




            });



            await Task.WhenAll(tasks);

            // Xóa job sau khi hoàn thành
            //await scheduler.DeleteJob(currentJob.Key);
        }

    }


}

