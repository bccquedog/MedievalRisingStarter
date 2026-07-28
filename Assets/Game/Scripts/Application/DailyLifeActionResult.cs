using System;

namespace MedievalRising.Application
{
    public enum DailyLifeActionKind
    {
        Eat,
        Work,
        BuyMeal,
        Sleep
    }

    public readonly struct DailyLifeActionResult : IEquatable<DailyLifeActionResult>
    {
        public DailyLifeActionResult(bool success, DailyLifeActionKind kind, string message)
        {
            Success = success;
            Kind = kind;
            Message = string.IsNullOrWhiteSpace(message) ? kind.ToString() : message;
        }

        public bool Success { get; }

        public DailyLifeActionKind Kind { get; }

        public string Message { get; }

        public bool Equals(DailyLifeActionResult other) =>
            Success == other.Success && Kind == other.Kind && Message == other.Message;

        public override bool Equals(object obj) => obj is DailyLifeActionResult other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Success, Kind, Message);

        public override string ToString() => $"{Kind}: {(Success ? "OK" : "FAIL")} — {Message}";
    }
}
