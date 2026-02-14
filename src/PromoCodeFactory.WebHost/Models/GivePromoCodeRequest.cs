using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.WebHost.Models
{
    /// <summary>
    /// Запрос на выдачу промокода клиенту. Это ИИ, детка
    /// </summary>
    public class GivePromoCodeRequest
    {
        /// <summary>
        /// Идентификатор клиента, которому выдается промокод
        /// </summary>
        [Required(ErrorMessage = "Идентификатор клиента обязателен")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Идентификатор предпочтения, на основе которого выдается промокод
        /// </summary>
        [Required(ErrorMessage = "Идентификатор предпочтения обязателен")]
        public Guid PreferenceId { get; set; }

        /// <summary>
        /// Идентификатор партнера, предоставляющего промокод
        /// </summary>
        [Required(ErrorMessage = "Идентификатор партнера обязателен")]
        public Guid PartnerId { get; set; }
    }
}