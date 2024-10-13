using Microsoft.AspNetCore.Http;
using MobileProgramming.Business.Models.Constants;
using MobileProgramming.Data.Entities;
using Newtonsoft.Json;
using System.Text;

namespace MobileProgramming.API.Helper
{
    public static class SessionHelper
    {
        public static void SetSession<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetSession<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void AddToCart(this ISession session, Product? product)
        {
            var carts = GetSession<HashSet<CartItem>>(session, SessionConstant.CartSession) ?? new HashSet<CartItem>();

            var cart = carts.FirstOrDefault(c => c.ProductId == product?.ProductId);

            if (cart == null)
            {
                cart = new CartItem
                {
                    ProductId = product!.ProductId,
                    Product = product!,
                    Price = product!.Price,
                    Quantity = 1
                };
                carts.Add(cart);
            }
            else
            {
                cart.Quantity++;
            }

            session.SetSession(SessionConstant.CartSession, carts);
        }


        public static void DeleteCart(this ISession session, int productId)
        {
            var carts = GetSession<HashSet<CartItem>>(session, SessionConstant.CartSession);

            if (carts == null)
            {
                return;
            }

            var cart = carts.FirstOrDefault(c => c.ProductId == productId);

            if (cart == null)
            {
                return;
            }

            carts.Remove(cart);

            session.SetSession(SessionConstant.CartSession, carts);
        }

        public static void DescreaseUnit(this ISession session, int productId)
        {
            var carts = GetSession<HashSet<CartItem>>(session, SessionConstant.CartSession);

            if (carts == null)
            {
                return;
            }

            var cart = carts.FirstOrDefault(c => c.ProductId == productId);

            if (cart == null)
            {
                return;
            }

            if (cart.Quantity == 1)
            {
                carts.Remove(cart);
            }
            else
            {
                cart.Quantity--;
            }

            session.SetSession(SessionConstant.CartSession, carts);
        }

        public static void IncreaseUnit(this ISession session, int productId)
        {
            var carts = GetSession<HashSet<CartItem>>(session, SessionConstant.CartSession);

            if (carts == null)
            {
                return;
            }

            var cart = carts.FirstOrDefault(c => c.ProductId == productId);

            if (cart == null)
            {
                return;
            }

            cart.Quantity++;

            session.SetSession(SessionConstant.CartSession, carts);
        }
    }
}
