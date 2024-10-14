using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Business.Models.ResponseMessage;
using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase
{
    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, APIResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddCartItemCommandHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = request.UserId,
                    Status = "active",
                    TotalPrice = 0 // Will update after adding items
                };

                await _cartRepository.Add(cart);
            }

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.CartId, request.ProductId);


            var product = await _productRepository.GetById(request.ProductId);
            if (product == null)
            {
                response.StatusResponse = HttpStatusCode.NotFound;
                response.Message = MessageProduct.ProductNotFound;
                response.Data = false;
            }


            if (cartItem != null)
            {
                
                cartItem.Quantity += request.Quantity;
               
            }
            else
            {
                
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price 
                };

                await _cartItemRepository.Add(cartItem);
            }

            var finalCartItem = _mapper.Map<CartItemDto>(cartItem);
            response.StatusResponse = HttpStatusCode.OK;
            response.Message = MessageCommon.SavingSuccesfully;
            response.Data = finalCartItem;

            return response;
        }
    }
}
