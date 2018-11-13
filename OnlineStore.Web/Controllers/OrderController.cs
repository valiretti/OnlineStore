using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class OrderController : Controller
    {
        IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public ActionResult Order()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                UserDto userDto = orderService.GetUserData(userId);
                if (userDto != null)
                {
                    var orderViewModel = new OrderViewModelForCart()
                    {
                        Name = userDto.Name,
                        Address = userDto.Address,
                        Email = userDto.Email,
                        PhoneNumber = userDto.PhoneNumber
                    };
                    return View(orderViewModel);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderViewModelForCart order)
        {
            if (ModelState.IsValid)
            {
                if (order.Items != null)
                {
                    List<LineItemViewModel> items = JsonConvert.DeserializeObject<List<LineItemViewModel>>(order.Items);

                    List<LineItemDto> lineItemDtos = new List<LineItemDto>();

                    foreach (var item in items)
                    {
                        var lineItemDto = new LineItemDto()
                        {
                            Count = item.Count,
                            ProductId = item.ProductId,
                        };
                        lineItemDtos.Add(lineItemDto);
                    }

                    var orderDto = new OrderDto
                    {
                        Name = order.Name,
                        Address = order.Address,
                        Email = order.Email,
                        PhoneNumber = order.PhoneNumber,
                    };

                    string userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;

                    orderService.MakeOrder(orderDto, lineItemDtos, userId);
                }

                return View("AcceptOrder");
            }

            return View();
        }

        public ActionResult GetOrders()
        {
            IEnumerable<OrderDto> orderDtos = orderService.GetOrders();
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderDto>, List<OrderViewModel>>(orderDtos);
            return View(orders);
        }

        public ActionResult GetOrdersForUser()
        {
            var userId = User.Identity.GetUserId();
            IEnumerable<OrderDto> orderDtos = orderService.GetOrdersForUser(userId);
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderDto>, List<OrderViewModel>>(orderDtos);
            return View("GetOrders", orders);
        }


        public ActionResult GetLineItems(int? id)
        {
            if (id != null)
            {
                IEnumerable<LineItemDto> lineItemDtosDtos = orderService.GetLineItemDtos(id.Value);
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItemDto, LineItemViewModel>()
                    .ForMember(x => x.ProductViewModel, s => s.MapFrom(t => t.ProductDto))
                    .ForMember(t => t.OrderViewModel, s => s.MapFrom(x => x.OrderDto))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()).CreateMapper();
                var lineItems = mapper.Map<IEnumerable<LineItemDto>, List<LineItemViewModel>>(lineItemDtosDtos);
                return View(lineItems);
            }

            return RedirectToAction("GetOrders");
        }

        [HttpGet]
        public ActionResult EditOrder(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    SelectList states = new SelectList(new[] { "Сonfirmed", "Odered", "Canceled" });
                    ViewBag.States = states;

                    return View(order);
                }
            }
            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult EditOrder(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<OrderViewModel, OrderDto>()).CreateMapper();
                var orderDto = mapper.Map<OrderViewModel, OrderDto>(orderViewModel);

                orderService.EditOrder(orderDto);
                return RedirectToAction("GetOrders");
            }

            SelectList states = new SelectList(new[] { "Сonfirmed", "Odered", "Canceled" });
            ViewBag.States = states;
            return View(orderViewModel);
        }

        [HttpGet]
        public ActionResult EditOrderForUser(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);

                if (orderDto != null)
                {
                    if (orderDto.State == "Сonfirmed")
                    {
                        return View("OrderConfirmed");
                    }
                    if (orderDto.State == "Canceled")
                    {
                        return View("OrderCanceled");
                    }

                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    SelectList states = new SelectList(new[] { "Odered", "Canceled" });
                    ViewBag.States = states;

                    return View("EditOrder", order);
                }
            }
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpPost]
        public ActionResult EditOrderForUser(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<OrderViewModel, OrderDto>()).CreateMapper();
                var orderDto = mapper.Map<OrderViewModel, OrderDto>(orderViewModel);

                orderService.EditOrder(orderDto);
                return RedirectToAction("GetOrdersForUser");
            }

            SelectList states = new SelectList(new[] { "Odered", "Canceled" });
            ViewBag.States = states;
            return View("EditOrder", orderViewModel);
        }

        [HttpGet]
        public ActionResult DeleteOrder(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    return View(order);
                }
            }

            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult DeleteOrder(OrderViewModel orderViewModel)
        {
            orderService.DeleteOrder(orderViewModel.Id);
            return RedirectToAction("GetOrders");
        }

        [HttpGet]
        public ActionResult DeleteOrderForUser(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    if (orderDto.State == "Сonfirmed")
                    {
                        return View("OrderConfirmed");
                    }

                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    return View("DeleteOrder", order);
                }
            }
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpPost]
        public ActionResult DeleteOrderForUser(OrderViewModel orderViewModel)
        {
            orderService.DeleteOrder(orderViewModel.Id);
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpGet]
        public ActionResult DeleteLineItem(int? id)
        {
            if (id != null)
            {
                LineItemDto lineItemDto = orderService.GetLineItemDto(id.Value);
                if (lineItemDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<LineItemDto, LineItemViewModel>()
                            .ForMember(x => x.ProductViewModel, s => s.MapFrom(t => t.ProductDto))).CreateMapper();
                    var lineItem = mapper.Map<LineItemDto, LineItemViewModel>(lineItemDto);

                    return View(lineItem);
                }
            }
            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult DeleteLineItem(LineItemViewModel lineItemViewModel)
        {
            orderService.DeleteLineItem(lineItemViewModel.Id);
            return RedirectToAction("GetOrders");
        }
    }
}