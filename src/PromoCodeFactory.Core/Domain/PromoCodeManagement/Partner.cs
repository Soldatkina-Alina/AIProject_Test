using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Партнер. Это ИИ, детка
    /// </summary>
    public class Partner
        : BaseEntity
    {
        /// <summary>
        /// Название партнера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выданных промокодов
        /// </summary>
        public int NumberIssuedPromoCodes { get; set; }

        /// <summary>
        /// Активность партнера
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Лимиты на промокоды для партнера
        /// </summary>
        public virtual ICollection<PartnerPromoCodeLimit> PartnerLimits { get; set; }

        /// <summary>
        /// Менеджер партнера
        /// </summary>
        public virtual Employee PartnerManager { get; set; }
    }
}
