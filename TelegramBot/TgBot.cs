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
using Update = Telegram.Bot.Types.Update;

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
		string trainerAnswer = "";
		public TgBot(IServiceScopeFactory scopeFactory)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				 .AddJsonFile("appsettings.json")
				 .Build();
			bot = new TelegramBotClient(configuration["TelgramToken"]);
			bot.StartReceiving(Update, Error);
			db = new DbFactory(scopeFactory);
		}
		private Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
		{
			throw new Exception("TelegramBot exeption", arg2);
		}

		private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
		{
			Message msg = update.Message;
			CallbackQuery answer = update.CallbackQuery;
			DaoClient daoClient = new DaoClient(db.GetContext());
			Client test = null;
			if (msg != null)
			{
				test = await daoClient.GetByChatIdAsync(msg.Chat.Id);
			}
			if (msg != null && test != null)
			{
				List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
				InlineKeyboardButton entry = new InlineKeyboardButton("Записаться");
				InlineKeyboardButton cancelEntry = new InlineKeyboardButton("Отменить запись");
				entry.CallbackData = entryAnswer;
				buttons.Add(entry);
				cancelEntry.CallbackData = cancelEntryAnswer;
				buttons.Add(cancelEntry);
				InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
				botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите действие", replyMarkup: ikm);
			}
			// trainer
			//else if (answer != null && answer.Data == entryAnswer)
			//{
			//	await botClient.SendTextMessageAsync(answer.Message.Chat.Id, "Выберите тренера", replyMarkup: GetTrainerButtons());
			//}
			//Date
			else if (answer != null && answer.Data == entryAnswer)
			{
				await botClient.SendTextMessageAsync(answer.Message.Chat.Id, "Выберите дату", replyMarkup: GetDateButtons());
			}
			//back
			else if (answer != null && answer.Data == backAnswer)
			{
				List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
				InlineKeyboardButton entry = new InlineKeyboardButton("Записаться");
				InlineKeyboardButton cancelEntry = new InlineKeyboardButton("Отменить запись");
				entry.CallbackData = entryAnswer;
				buttons.Add(entry);
				cancelEntry.CallbackData = cancelEntryAnswer;
				buttons.Add(cancelEntry);
				InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
				await botClient.SendTextMessageAsync(answer.Message.Chat.Id, "Выберите дату", replyMarkup: ikm);
			}
			// time
			else if (answer != null && answer.Data.Contains(dateAnswer))
			{
				string date = answer.Data.Trim(dateAnswer.ToCharArray());
				await botClient.SendTextMessageAsync(answer.Message.Chat.Id,
					"Выберите время:", replyMarkup: GetTimeButtons(date));
			}
			//reception
			else if (answer != null && answer.Data.Contains(timeAnswer))
			{

				string date = answer.Data.Trim(timeAnswer.ToCharArray());
				DateTime resultDate = DateTime.Parse(date);
				DaoTraining daoTraining = new DaoTraining(db.GetContext());
				Training training = await daoTraining.FindToDateAsync(DateOnly.FromDateTime(resultDate));
				Client client = await daoClient.GetByChatIdAsync(update.CallbackQuery.Message.Chat.Id);

				training.Clients.Add(client);
				daoTraining.UpdateAsync(training);
				await botClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id,
					client.FirstName + client.LastName + " записаны на: " + resultDate);

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

		//private IReplyMarkup? GetTrainerButtons()
		//{
		//	List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
		//	List<Worker> workers = db.GetContext().Workers.ToList();

		//	foreach (Worker trainer in workers)
		//	{
		//		string nameButton = trainer.LastName;
		//		InlineKeyboardButton button = new InlineKeyboardButton(nameButton);
		//		button.CallbackData = trainer.Id.ToString(dateAnswer + nameButton);
		//		buttons.Add(button);
		//	}

		//	InlineKeyboardButton back = new InlineKeyboardButton("Назад");
		//	back.CallbackData = backAnswer;
		//	buttons.Add(back);
		//	InlineKeyboardMarkup ikm = new InlineKeyboardMarkup(buttons);
		//	return ikm;
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
			List<Training> trainings = db.GetContext().Workout.ToList();

			foreach (Training trainingItem in trainings)
			{
				if (trainingItem.Time.Date.ToString(dateFormat) == date)
				{

					string nameButton = trainingItem.Time.ToString("HH-mm");
					InlineKeyboardButton button = new InlineKeyboardButton(nameButton);
					button.CallbackData = trainingItem.Time.ToString(timeAnswer + trainingItem.Time.ToString());
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

