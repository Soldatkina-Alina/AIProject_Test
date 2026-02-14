using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Промокод. Это ИИ, детка
    /// </summary>
    public class PromoCode
        : BaseEntity
    {
        /// <summary>
        /// Код промокода
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Информация о сервисе
        /// </summary>
        public string ServiceInfo { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Название партнера
        /// </summary>
        public string PartnerName { get; set; }

        /// <summary>
        /// Менеджер партнера
        /// </summary>
        public virtual Employee PartnerManager { get; set; }

        /// <summary>
        /// Предпочтение, для которого выдан промокод
        /// </summary>
        public virtual Preference Preference { get; set; }

        /// <summary>
        /// Генерирует уникальный код промокода
        /// </summary>
        /// <returns>Уникальный код промокода в формате [A-Z]{4}-[0-9]{6}</returns>
        public static string GenerateCode()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            var random = new Random();

            // Генерация 4 букв
            var codePart1 = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                codePart1.Append(letters[random.Next(letters.Length)]);
            }

            // Генерация 6 цифр
            var codePart2 = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                codePart2.Append(digits[random.Next(digits.Length)]);
            }

            return $"{codePart1}-{codePart2}";
        }

        /// <summary>
        /// Генерирует случайную скидку от 10% до 50%
        /// </summary>
        /// <returns>Процент скидки</returns>
        public static int GenerateDiscount()
        {
            var random = new Random();
            return random.Next(10, 51);
        }

        /// <summary>
        /// Создает новый промокод с заданными параметрами
        /// </summary>
        /// <param name="partnerName">Название партнера</param>
        /// <param name="preference">Предпочтение</param>
        /// <param name="partnerManager">Менеджер партнера</param>
        /// <returns>Новый промокод</returns>
        public static PromoCode Create(string partnerName, Preference preference, Employee partnerManager)
        {
            if (string.IsNullOrEmpty(partnerName))
                throw new ArgumentNullException(nameof(partnerName), "Название партнера не может быть пустым");

            if (preference == null)
                throw new ArgumentNullException(nameof(preference), "Предпочтение не может быть null");

            if (partnerManager == null)
                throw new ArgumentNullException(nameof(partnerManager), "Менеджер партнера не может быть null");

            var promoCode = new PromoCode
            {
                Code = GenerateCode(),
                ServiceInfo = "Промокод для клиента",
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PartnerName = partnerName,
                Preference = preference,
                PartnerManager = partnerManager
            };

            return promoCode;
        }
    }
}
