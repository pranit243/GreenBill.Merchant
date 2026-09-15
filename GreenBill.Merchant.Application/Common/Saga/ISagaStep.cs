namespace GreenBill.Merchant.Application.Common.Saga
{
    /// <summary>
    /// Reference skeleton for an orchestration-style Saga (doc.md section 11).
    /// A saga is a sequence of steps, each with a compensating action, coordinated by a
    /// <see cref="ISagaOrchestrator{TContext}"/>. Wire real steps up once the async workflow
    /// (e.g. Billing -> Payment -> Notification) is implemented and RabbitMQ is introduced.
    /// </summary>
    public enum SagaStepStatus
    {
        Succeeded,
        Failed
    }

    public record SagaStepResult(SagaStepStatus Status, string? Reason = null)
    {
        public static SagaStepResult Success() => new(SagaStepStatus.Succeeded);
        public static SagaStepResult Failure(string reason) => new(SagaStepStatus.Failed, reason);
    }

    /// <summary>A single forward action + compensating (rollback) action in a saga.</summary>
    public interface ISagaStep<TContext>
    {
        string Name { get; }

        Task<SagaStepResult> ExecuteAsync(TContext context, CancellationToken cancellationToken);

        Task CompensateAsync(TContext context, CancellationToken cancellationToken);
    }
}
