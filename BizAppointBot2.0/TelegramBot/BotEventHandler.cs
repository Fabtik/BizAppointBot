using Azure.Core;
using BLL.Models.Requests;
using BLL.Services.Abstract;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot;

namespace TelegramBot
{
    public class BotEventHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppointmentService _appointmentService;
        public BotEventHandler(ITelegramBotClient botClient, IServiceProvider serviceProvider, IAppointmentService appointmentService)
        {
            _botClient = botClient;
            _serviceProvider = serviceProvider;
            _appointmentService = appointmentService;

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Console.WriteLine($"Bot listening for updates.");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {
                await HandleMessage(botClient, update.Message);
                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: BotConfiguration.OwnerChatId,
                text: "You said:\n" + update.Message,
                cancellationToken: cancellationToken);
        }

        private async Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, ".-.");
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, $"You said:\n{message.Text}");
        }

        private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("confirm"))
            {
                // Отримано підтвердження, відправляємо OperationConfirmationData у сервіс
                Console.WriteLine($"{callbackQuery.Data}");
                int appointmentId = int.Parse(callbackQuery.Data.Substring(8));

                var appointment = await _appointmentService.GetByIdAsync(appointmentId);

                if (await _appointmentService.TryToConfirmAppointment(appointmentId))
                {
                    await _botClient.SendTextMessageAsync(
                        BotConfiguration.OwnerChatId, 
                        $"Запис {appointment.Name} на {appointment.AppointedTime.ToString("dd-MM-yyyy HH:mm:ss")} підтверджено");

                    return;
                }


                await _botClient.SendTextMessageAsync(
                    BotConfiguration.OwnerChatId, 
                    $"Не вдалося підтвердити запис {appointment.Name} на {appointment.AppointedTime.ToString("dd-MM-yyyy HH:mm:ss")}");

                return;                
            }
            if (callbackQuery.Data.StartsWith("delete"))
            {
                Console.WriteLine($"{callbackQuery.Data}");
                int appointmentId = int.Parse(callbackQuery.Data.Substring(7));

                var appointment = await _appointmentService.GetByIdAsync(appointmentId);

                if(await _appointmentService.TryToDeleteAsync(appointmentId))
                {
                    await botClient.SendTextMessageAsync(
                        BotConfiguration.OwnerChatId,
                        $"Видалено запис {appointment.Name} на {appointment.AppointedTime.ToString("dd-MM-yyyy HH:mm:ss")}" //add extra confirm?
                    );

                    return;
                }

                await botClient.SendTextMessageAsync(
                        BotConfiguration.OwnerChatId,
                        $"Не вдалося видалити запис {appointment.Name} на {appointment.AppointedTime.ToString("dd-MM-yyyy HH:mm:ss")}" //add extra confirm?
                    );

                return;
            }
            await botClient.SendTextMessageAsync(
                BotConfiguration.OwnerChatId,
                $"You chose with data: {callbackQuery.Data}"
            );

        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }

    
}
