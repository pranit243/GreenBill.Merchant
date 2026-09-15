using Microsoft.Extensions.Logging;

namespace GreenBill.Merchant.Application.Common.Saga
{
    /// <summary>
    /// Executes an ordered list of <see cref="ISagaStep{TContext}"/> and compensates
    /// already-completed steps (in reverse order) if a later step fails.
    /// See doc.md section 11 (Saga Pattern Recommendation) for the target workflow.
    /// </summary>
    public class SagaOrchestrator<TContext>
    {
        private readonly IReadOnlyList<ISagaStep<TContext>> _steps;
        private readonly ILogger<SagaOrchestrator<TContext>> _logger;

        public SagaOrchestrator(
            IReadOnlyList<ISagaStep<TContext>> steps,
            ILogger<SagaOrchestrator<TContext>> logger)
        {
            _steps = steps;
            _logger = logger;
        }

        public async Task<SagaStepResult> RunAsync(TContext context, CancellationToken cancellationToken)
        {
            var completed = new Stack<ISagaStep<TContext>>();

            foreach (var step in _steps)
            {
                var result = await step.ExecuteAsync(context, cancellationToken);

                if (result.Status == SagaStepStatus.Failed)
                {
                    _logger.LogWarning(
                        "Saga step {StepName} failed: {Reason}. Compensating {CompletedCount} step(s).",
                        step.Name,
                        result.Reason,
                        completed.Count);

                    while (completed.Count > 0)
                    {
                        var toCompensate = completed.Pop();
                        await toCompensate.CompensateAsync(context, cancellationToken);
                    }

                    return result;
                }

                completed.Push(step);
            }

            return SagaStepResult.Success();
        }
    }
}
