using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridUI.Persistance;
using Incoding.Core.CQRS.Core;

namespace GridUI.Operations
{
    public class AddProductCommand : CommandBaseAsync
    {
        #region Properties

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public bool IsSoldOut { get; set; }

        public List<User> Users { get; set; }

        #endregion

        protected override async Task ExecuteAsync()
        {
            var product = new Product();
            product.Name = Name;
            product.Price = Price;
            product.Date = Date;
            product.IsSoldOut = IsSoldOut;
            product.Users = Users;
            await Repository.SaveAsync(product);
        }

    }
}