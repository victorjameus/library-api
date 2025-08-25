namespace Library.Domain.ValueObjects
{
    public sealed record ISBN
    {
        public string Value { get; }

        private ISBN(string value)
        {
            Value = value;
        }

        public static ISBN Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ISBN no puede estar vacío");
            }

            var clean = value.Replace("-", "").Replace(" ", "");

            if (clean.Length is not 10 and 13)
            {
                throw new ArgumentException("ISBN debe tener 10 o 13 dígitos");
            }

            return new ISBN(clean);
        }

        public string GetFormattedISBN()
        {
            return Value.Length is 13
                ? $"{Value[..3]}-{Value[3]}-{Value[4..6]}-{Value[6..12]}-{Value[12]}"
                : $"{Value[..1]}-{Value[1..4]}-{Value[4..9]}-{Value[9]}";
        }

        public override string ToString() => Value;
    }
}
