using MediatR;
using MobileProgramming.Business.UseCase.Order.Queries.QueryOrder;
using MobileProgramming.Data.Interfaces.Common;
using Quartz;

namespace MobileProgramming.Business.Quartz.PaymentScheduler
{
    public class CheckTransactionStatusJob : IJob
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly IMediator _mediator;
        private readonly IRedisCaching _caching;

        public CheckTransactionStatusJob(ISchedulerFactory scheduler, IMediator mediator, IRedisCaching caching)
        {
            _scheduler = scheduler;
            _mediator = mediator;
            _caching = caching;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;
            List<string> cacheKeys = new List<string>();

            cacheKeys = await _caching.SearchKeysAsync("payment_pending");

            //handle if redis crash
            //if (cacheKeys == null)
            //{
            //    var processingTransaction = await _transactionRepository.getProcessingTransaction();
            //    cacheKeys = (List<string>)processingTransaction;
            //}


            var tasks = cacheKeys.Select(async key =>
            {
                string? app_trans_id = null;

                app_trans_id = await _caching.HashGetSpecificKeyAsync(key, "transactionId");

                var queryOrder = await _mediator.Send(new QueryOrder(app_trans_id));

                if (queryOrder.StatusResponse != System.Net.HttpStatusCode.OK)
                {
                    return;
                }
                await _caching.DeleteKeyAsync($"payment_pending_{app_trans_id}");


            });



            await Task.WhenAll(tasks);
        }

    }


}

