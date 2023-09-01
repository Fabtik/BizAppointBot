using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using DAL.Entities;
using BLL.Models.Requests;
using System;
using System.Text.Json;

namespace TelegramBot
{
    public static class BotRequestHandler
    {
        private static ITelegramBotClient _botClient;
        private static BotEventHandler _botHandler;

        public static void Initialize(ITelegramBotClient botClient, IServiceProvider serviceProvider)
        {
            _botClient = botClient;

            var botEventHandler = serviceProvider.GetRequiredService<BotEventHandler>();

            _botHandler = botEventHandler;
        }


        public static async Task ConfirmRequest(AppointmentRequest appointment, int appointmentId)
        {
            int chatId = int.Parse(BotConfiguration.OwnerChatId);        

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"Підтвердити", $"confirm {appointmentId}"),
                    InlineKeyboardButton.WithCallbackData($"Видалити", $"delete {appointmentId}"),
                },
            });

            await _botClient.SendTextMessageAsync(BotConfiguration.OwnerChatId, $"{appointment.Name} | {appointment.PhoneNumber} | {appointment.AppointedTime}", replyMarkup: inlineKeyboard);


            //await _botClient.SendTextMessageAsync(chatId, confirmationMessage);
        }

    }
}
