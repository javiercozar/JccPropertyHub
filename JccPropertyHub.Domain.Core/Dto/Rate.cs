namespace JccPropertyHub.Domain.Core.Dto {
    public class Rate {
        public string RatePlanId { get; set; }
        public int MealPlanId { get; set; }
        public bool IsCancellable { get; set; }
        public double Price { get; set; }
    }
}