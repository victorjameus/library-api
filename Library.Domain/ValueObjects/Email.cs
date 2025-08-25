namespace Library.Domain.ValueObjects
{
    public sealed record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email no puede estar vacío");
            }

            if (!IsValidEmail(value))
            {
                throw new ArgumentException("Formato de email inválido");
            }

            return new Email(value.ToLowerInvariant());
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);

                return mail.Address.Equals(email);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString() => Value;
    }
}
