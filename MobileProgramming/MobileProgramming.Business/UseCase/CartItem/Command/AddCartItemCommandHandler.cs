using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using MobileProgramming.Data.Interfaces.Common;
using System.Net;

namespace MobileProgramming.Business.UseCase
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, APIResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddCartItemCommandHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

            if (cart != null)
            {
                await _cartRepository.Delete(cart.CartId);
                await _unitOfWork.SaveChangesAsync();
            }

            var newCart = new Cart
            {
                UserId = request.UserId,
                Status = "active",
                TotalPrice = 0 // Will update after adding items
            };

            await _cartRepository.Add(newCart);
            await _unitOfWork.SaveChangesAsync();


            var cartItems = new List<CartItem>();
            var existedCart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
            for (int i = 0; i < request.ProductId.Count; i++)
            {
                var product = await _productRepository.GetById(request.ProductId[i]);
                var cartItem = new CartItem
                {
                    CartId = existedCart.CartId,
                    ProductId = request.ProductId[i],
                    Quantity = request.Quantity[i],
                    Price = product.Price * request.Quantity[i]

                };
                existedCart.TotalPrice += cartItem.Price;
                cartItems.Add(cartItem);
            }

            // Add all cart items to the repository
            foreach (var item in cartItems)
            {
                await _cartItemRepository.Add(item);
                
            }
            await _unitOfWork.SaveChangesAsync();

            //if (cart == null)
            //{
            //    cart = new Cart
            //    {
            //        UserId = request.UserId,
            //        Status = "active",
            //        TotalPrice = 0 // Will update after adding items
            //    };

            //    await _cartRepository.Add(cart);
            //    await _unitOfWork.SaveChangesAsync();
            //}

            //var cartItem = await _cartItemRepository.GetCartItemAsync(cart.CartId, request.ProductId);


            //var product = await _productRepository.GetById(request.ProductId);
            //if (product == null)
            //{
            //    response.StatusResponse = HttpStatusCode.NotFound;
            //    response.Message = MessageProduct.ProductNotFound;
            //    response.Data = false;
            //}


            //if (cartItem != null)
            //{

            //    cartItem.Quantity += request.Quantity;

            //}
            //else
            //{

            //    cartItem = new CartItem
            //    {
            //        CartId = cart.CartId,
            //        ProductId = request.ProductId,
            //        Quantity = request.Quantity,
            //        Price = product.Price 
            //    };

            //    await _cartItemRepository.Add(cartItem);
            //    await _unitOfWork.SaveChangesAsync();
            //}

            //var finalCartItem = _mapper.Map<CartItemDto>(cartItem);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.SavingSuccesfully;
            response.Data = request;

            return response;
        }
    }
}
