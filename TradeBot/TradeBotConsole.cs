﻿using IBApi;
using System;
using System.Collections.Generic;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.FileIO;
using TradeBot.Generated;
using TradeBot.Gui;
using TradeBot.MenuFramework;
using TradeBot.TwsAbstractions;
using TradeBot.Utils;
using static TradeBot.GlobalProperties;
using static TradeBot.Gui.Window;

namespace TradeBot
{
    public class TradeBotConsole
    {
        public static void Main(string[] args)
        {
            TradeBotConsole console = new TradeBotConsole();
            console.Start();
        }

        private readonly IList<int> ignoredErrorCodes = new List<int>()
        {
            ErrorCodes.MARKET_DATA_FARM_DISCONNECTED,
            ErrorCodes.MARKET_DATA_FARM_CONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_DISCONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_CONNECTED,
            ErrorCodes.HISTORICAL_DATA_FARM_INACTIVE,
            ErrorCodes.MARKET_DATA_FARM_INACTIVE
        };

        private TradeBotService service;
        private Menu menu;

        private bool shouldExitApplication;

        public TradeBotConsole()
        {
            service = new TradeBotService();

            InitializeEventHandlers();
            InitializeConsole();
            InitializeMenu();
        }

        #region Initialization
        private void InitializeEventHandlers()
        {
            Window.SetWindowCloseHandler(OnWindowClose);

            service.PropertyValueChanged += OnPropertyValueChanged;
            service.ConnectionClosed += OnConnectionClosed;
            service.TickDataUpdated += OnTickDataUpdated;
            service.ErrorOccured += OnError;
        }

        private void InitializeConsole()
        {
            Console.Title = Messages.AppName;
            // Set the console buffer height to the maximum allowed value.
            Console.BufferHeight = Int16.MaxValue - 1;
            if (Preferences.CenterWindow)
            {
                Window.SetWindowSizeAndCenter(
                    Preferences.WindowWidth,
                    Preferences.WindowHeight);
            }
        }

        private void InitializeMenu()
        {
            menu = new Menu();

            Action<IList<string>, Action> addMenuOption = (entry, command)
                => menu.AddMenuOption(entry[0], entry[1], command);

            MenuOptionEntries menuOptionEntry = Messages.MenuOptionEntries;
            addMenuOption(menuOptionEntry.ReloadSavedState, ReloadSavedStateCommand);
            addMenuOption(menuOptionEntry.SetTickerSymbol, SetTickerSymbolDataCommand);
            addMenuOption(menuOptionEntry.ClearTickerSymbol, ClearTickerSymbolCommand);
            addMenuOption(menuOptionEntry.SetStepSize, SetStepSizeCommand);
            addMenuOption(menuOptionEntry.SetStepSizeFromCash, SetStepSizeFromCashCommand);
            addMenuOption(menuOptionEntry.Buy, BuyCommand);
            addMenuOption(menuOptionEntry.Sell, SellCommand);
            addMenuOption(menuOptionEntry.ReversePosition, ReversePositionCommand);
            addMenuOption(menuOptionEntry.ClosePosition, ClosePositionCommand);
            addMenuOption(menuOptionEntry.Misc, MiscCommand);
            addMenuOption(menuOptionEntry.ClearScreen, ClearScreenCommand);
            addMenuOption(menuOptionEntry.Help, HelpCommand);
            addMenuOption(menuOptionEntry.ExitApplication, ExitApplicationCommand);
        }
        #endregion

        #region Public methods
        public void Start()
        {
            IO.ShowMessage(Messages.WelcomeMessage);

            try
            {
                service.Connect();
                LoadState();
                while (!shouldExitApplication)
                {
                    menu.PromptForMenuOption().Command();
                }
            }
            catch (Exception e)
            {
                IO.ShowMessage(e.Message, MessageType.ERROR);
            }
            finally
            {
                Shutdown();
                if (OS.IsWindows())
                {
                    IO.PromptForChar(Messages.PressAnyKeyToExit);
                }
            }
        }
        #endregion

        #region Menu commands
        private void ReloadSavedStateCommand()
        {
            LoadState();
        }

        private void SetTickerSymbolDataCommand()
        {
            string tickerSymbol = IO.PromptForInput(Messages.SelectTickerPrompt);
            IfNotNullOrWhiteSpace(tickerSymbol)(() =>
            {
                service.TickerSymbol = tickerSymbol;
            });
        }

        private void ClearTickerSymbolCommand()
        {
            service.TickerSymbol = null;
        }

        private void SetStepSizeCommand()
        {
            string stepSizeString = IO.PromptForInput(Messages.StepSizePrompt);
            int? stepSize = stepSizeString.ToInt();
            IfHasValue(stepSize)(() =>
            {
                service.StepSize = stepSize.Value;
            });
        }

        private void SetStepSizeFromCashCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                string cashString = IO.PromptForInput(Messages.StepSizeFromCashPrompt);
                double? cash = cashString.ToDouble();
                IfHasValue(cash)(() =>
                {
                    double sharePrice = service.GetTickData(TickType.LAST);
                    service.StepSize = StockMath.CalculateStepSizeFromCashValue(cash.Value, sharePrice);
                });
            });
        }

        private void BuyCommand()
        {
            Validator[] validators = { IfTickerSet(), IfStepSizeSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                service.PlaceOrder(OrderActions.BUY, service.StepSize, service.GetTickData(TickType.ASK));
            });
        }

        private void SellCommand()
        {
            Validator[] validators = { IfTickerSet(), IfStepSizeSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
                service.PlaceOrder(OrderActions.SELL, service.StepSize, service.GetTickData(TickType.BID));
            });
        }

        private void ReversePositionCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, async () =>
            {
                IDictionary<string, PositionInfo> positions = await service.GetPositions();
                PositionInfo position;
                if (positions.TryGetValue(service.TickerSymbol, out position))
                {

                }
            });
        }

        private void ClosePositionCommand()
        {
            Validator[] validators = { IfTickerSet(), IfPriceDataAvailable() };
            Validate(validators, () =>
            {
            });
        }

        private void MiscCommand()
        {
        }

        private void ClearScreenCommand()
        {
            Console.Clear();
        }

        private void HelpCommand()
        {
            IO.ShowMessage(menu.ToString());
        }

        private void ExitApplicationCommand()
        {
            shouldExitApplication = true;
        }
        #endregion

        #region Event handlers
        private bool OnWindowClose(CloseReason reason)
        {
            Shutdown();

            // return false since we didn't handle the control signal, 
            // i.e. Environment.Exit(-1);
            return false;
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(service.ManagedAccounts):
                    OnManagedAccountsChanged(eventArgs);
                    break;
                case nameof(service.TickerSymbol):
                    OnTickerChanged(eventArgs);
                    break;
                case nameof(service.StepSize):
                    OnStepSizeChanged(eventArgs);
                    break;
            }
        }

        private void OnManagedAccountsChanged(PropertyValueChangedEventArgs eventArgs)
        {
            string[] accounts = eventArgs.NewValue as string[];
            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    if (account.StartsWith("D", StringComparison.InvariantCulture))
                    {
                        IO.ShowMessage(Messages.AccountTypePaper, MessageType.SUCCESS);
                    }
                    else
                    {
                        IO.ShowMessage(Messages.AccountTypeLive, MessageType.WARNING);
                    }
                }
            }
        }

        private void OnTickerChanged(PropertyValueChangedEventArgs eventArgs)
        {
            string oldValue = eventArgs.OldValue as string;
            if (!string.IsNullOrWhiteSpace(oldValue))
            {
                IO.ShowMessage(Messages.TickerSymbolClearedFormat, oldValue);
            }
            string newValue = eventArgs.NewValue as string;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                IO.ShowMessage(Messages.TickerSymbolSetFormat, newValue);
            }
            else
            {
                Console.Title = string.Empty;
            }
        }

        private void OnStepSizeChanged(PropertyValueChangedEventArgs eventArgs)
        {
            IO.ShowMessage(Messages.StepSizeSetFormat, eventArgs.NewValue);
        }

        private void OnConnectionClosed()
        {
            IO.ShowMessage(Messages.TwsDisconnectedError, MessageType.ERROR);
        }

        private void OnTickDataUpdated(TickData tickData)
        {
            UpdateConsoleTitle(tickData);
        }

        private void OnError(int id, int errorCode, string errorMessage)
        {
            if (ignoredErrorCodes.Contains(errorCode))
            {
                return;
            }

            IO.ShowMessage(Messages.TwsErrorFormat, MessageType.ERROR, errorMessage);
        }
        #endregion

        #region Private methods
        private void Shutdown()
        {
            service.Disconnect();
            SaveState();
        }

        private void SaveState()
        {
            service.SaveState();
            IO.ShowMessage(Messages.SavedStateFormat, MessageType.SUCCESS, PropertyFiles.STATE_FILE);
        }

        private void LoadState()
        {
            IO.ShowMessage(Messages.LoadedStateFormat, MessageType.SUCCESS, PropertyFiles.STATE_FILE);
            service.LoadState();
        }

        private void UpdateConsoleTitle(TickData tickData)
        {
            IList<string> infoStrings = new List<string>();
            string appName = Messages.AppName;
            if (!string.IsNullOrWhiteSpace(appName))
            {
                infoStrings.Add(appName);
            }
            infoStrings.Add(string.Format(Messages.TitleTicker, service.TickerSymbol));
            infoStrings.Add(string.Format(Messages.TitleLastFormat, tickData[TickType.LAST]));
            infoStrings.Add(string.Format(Messages.TitleStepSize, service.StepSize));
            infoStrings.Add(string.Format(Messages.TitlePositionSize, 0));
            infoStrings.Add(string.Format(Messages.TitleBidAskFormat, tickData[TickType.BID], tickData[TickType.ASK]));
            infoStrings.Add(string.Format(Messages.TitleVolumeFormat, tickData[TickType.VOLUME]));
            infoStrings.Add(string.Format(Messages.TitleCloseFormat, tickData[TickType.CLOSE]));
            infoStrings.Add(string.Format(Messages.TitleOpenFormat, tickData[TickType.OPEN]));
            Console.Title = string.Join(Messages.TitleDivider, infoStrings);
        }
        #endregion

        #region Validations
        private delegate bool Validation();
        private delegate bool Validator(Action ifValidCallback);

        private void Validate(Validator[] validators, Action ifValidCallback)
        {
            bool valid = false;
            foreach (var validator in validators)
            {
                valid &= validator(null);
            }
            if (valid)
            {
                ifValidCallback();
            }
        }

        private Validator IfTickerSet()
        {
            return CreateValidator(
                () => service.TickerSymbol != null,
                Messages.TickerSymbolNotSetError);
        }

        private Validator IfStepSizeSet()
        {
            return CreateValidator(
                () => service.StepSize > 0,
                Messages.StepSizeNotSetError);
        }

        private Validator IfPriceDataAvailable()
        {
            Func<int, bool> ifAvailable = (tickType)
                => service.GetTickData(tickType) > 0;

            return CreateValidator(
                () => ifAvailable(TickType.LAST) && ifAvailable(TickType.ASK) && ifAvailable(TickType.BID),
                Messages.PriceDataUnavailableError);
        }

        private Validator IfNotNullOrWhiteSpace(string str)
        {
            return CreateValidator(
                () => !string.IsNullOrWhiteSpace(str),
                Messages.InvalidNonEmptyStringInputError);
        }

        private Validator IfHasValue(int? nullable)
        {
            return CreateValidator(
                () => nullable.HasValue,
                Messages.InvalidIntegerInputError);
        }

        private Validator IfHasValue(double? nullable)
        {
            return CreateValidator(
                () => nullable.HasValue,
                Messages.InvalidDecimalInputError);
        }

        private Validator CreateValidator(Validation validation, string errorMessage)
        {
            return (ifValidCallback) =>
            {
                bool valid = validation();
                if (valid)
                {
                    ifValidCallback?.Invoke();
                }
                else
                {
                    IO.ShowMessage(errorMessage, MessageType.VALIDATION_ERROR);
                }
                return valid;
            };
        }
        #endregion
    }
}
