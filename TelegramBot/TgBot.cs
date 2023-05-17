using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Model;
using TelegramBot.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Formats.Asn1.AsnWriter;

namespace TelegramBot
{

    public class TgBot
    {
        TelegramBotClient bot;
        DbFactory db;
        string timeAnswer = "CRONO: ";
        string dateFormat = "dd-MM-yy";
        string dateAnswer = "DATE: ";
        string entryAnswer = "entry";
        string cancelEntryAnswer = "cancelEntry";
        string backAnswer = "back";
        public TgBot(IServiceScopeFactory scopeFactory)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json")
                 .Build();
            bot = new TelegramBotClient(configuration["TelgramToken"]);
            bot.StartReceiving(UpdateBot, Error);
            db = new DbFactory(scopeFactory);
        }
        private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new Exception("TelegramBot exeption", arg2);
        }

        private async Task UpdateBot(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            Message msg = update.Message;
            CallbackQuery answer = update.CallbackQuery;
            DaoClient daoClient = new DaoClient(db.GetContext());
            if (msg != null && daoClient.GetByChatIdAsync(msg.Chat.Id) != null)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                InlineKeyboardButton entry = new InlineKeyboardButton("Записаться");
                InlineKeyboardButton cancelEntry = new InlineKeyboardButton("Отменить запись");
                entry.CallbackData = entryAnswer;
                buttons.Add(entry);
                cancelEntry.CallbackData = cancelEntryAnswer;
                buttons.Add(cancelEntry);
                InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
                botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите дату", replyMarkup: ikm);
            }

            else if (answer.Data == entryAnswer)
            {
                botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите дату", replyMarkup: GetDateButtons());
            }

            else if (answer.Data == backAnswer)
            {
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                InlineKeyboardButton entry = new InlineKeyboardButton("Записаться");
                InlineKeyboardButton cancelEntry = new InlineKeyboardButton("Отменить запись");
                entry.CallbackData = entryAnswer;
                buttons.Add(entry);
                cancelEntry.CallbackData = cancelEntryAnswer;
                buttons.Add(cancelEntry);
                InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
                botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите дату", replyMarkup: ikm);
            }
            else if (answer.Data.Contains(dateAnswer))
            {
                string date = answer.Data.Trim(dateAnswer.ToCharArray());
                botClient.SendTextMessageAsync(answer.Message.Chat.Id,
                   "Выберите время:", replyMarkup: GetTimeButtons(date));
            }
            else if (answer.Data.Contains(timeAnswer))
            {

                string date = answer.Data.Trim(timeAnswer.ToCharArray());
                DateTime resultDate = DateTime.Parse(date);
                DaoPlanForDay plan = new DaoPlanForDay(db.GetContext());
                PlanForDay planForDay = await plan.FindToDateAsync(DateOnly.FromDateTime(resultDate));
                Client client = await daoClient.GetByChatIdAsync(update.CallbackQuery.Message.Chat.Id);

                planForDay.Clients.Add(client);
                plan.UpdateAsync(planForDay);
                await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,
                    client.FirstName + client.LastName + " записаны на: " + date);

            }
            // TODO create func "Menu" for update, swich answer?
        }
        //void MainMenu(Message msg, ITelegramBotClient botClient)
        //{
        //    List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
        //    InlineKeyboardButton entry = new InlineKeyboardButton("Записаться");
        //    InlineKeyboardButton cancelEntry = new InlineKeyboardButton("Отменить запись");
        //    entry.CallbackData = entryAnswer;
        //    buttons.Add(entry);
        //    cancelEntry.CallbackData = cancelEntryAnswer;
        //    buttons.Add(cancelEntry);
        //    InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
        //    botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите дату", replyMarkup: ikm);
        //}

        //void MenuDate(Message msg, ITelegramBotClient botClient)
        //{
        //    botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите дату", replyMarkup: GetDateButtons());
        //}

        //void MenuTime(CallbackQuery answer, ITelegramBotClient botClient)
        //{
        //    string date = answer.Data.Trim(dateAnswer.ToCharArray());
        //    botClient.SendTextMessageAsync(answer.Message.Chat.Id,
        //       "Выберите время:", replyMarkup: GetTimeButtons(date));
        //}
        private IReplyMarkup? GetDateButtons()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            List<PlanForDay> plan = db.GetContext().PlanForDay.ToList();

            foreach (PlanForDay planItem in plan)
            {
                if (planItem.Date > DateTime.UtcNow)
                {
                    string nameButton = planItem.Date.Date.ToString(dateFormat);
                    InlineKeyboardButton button = new InlineKeyboardButton(nameButton);
                    button.CallbackData = planItem.Date.ToString(dateAnswer + nameButton);
                    buttons.Add(button);
                }
            }

            InlineKeyboardButton back = new InlineKeyboardButton("Назад");
            back.CallbackData = backAnswer;
            buttons.Add(back);
            InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
            return ikm;
        }

        private IReplyMarkup? GetTimeButtons(string date)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            List<PlanForDay> plan = db.GetContext().PlanForDay.ToList();

            foreach (PlanForDay planItem in plan)
            {
                if (planItem.Date.Date.ToString(dateFormat) == date)
                {
                    string nameButton = planItem.Date.ToString("HH-mm");
                    InlineKeyboardButton button = new InlineKeyboardButton(nameButton);
                    button.CallbackData = planItem.Date.ToString(timeAnswer + planItem.Date.ToString());
                    buttons.Add(button);
                }
            }

            InlineKeyboardButton back = new InlineKeyboardButton("Назад");
            back.CallbackData = backAnswer;
            buttons.Add(back);
            InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
            return ikm;
        }


    }
}

