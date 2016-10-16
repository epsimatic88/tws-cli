﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TradeBot.Generated
{

    public class MenuOptionEntries
    {

        [JsonProperty("reloadSavedState")]
        public IList<string> ReloadSavedState { get; set; }

        [JsonProperty("setTickerSymbol")]
        public IList<string> SetTickerSymbol { get; set; }

        [JsonProperty("clearTickerSymbol")]
        public IList<string> ClearTickerSymbol { get; set; }

        [JsonProperty("setStepSize")]
        public IList<string> SetStepSize { get; set; }

        [JsonProperty("setStepSizeFromCash")]
        public IList<string> SetStepSizeFromCash { get; set; }

        [JsonProperty("buy")]
        public IList<string> Buy { get; set; }

        [JsonProperty("sell")]
        public IList<string> Sell { get; set; }

        [JsonProperty("reversePosition")]
        public IList<string> ReversePosition { get; set; }

        [JsonProperty("closePosition")]
        public IList<string> ClosePosition { get; set; }

        [JsonProperty("listPositions")]
        public IList<string> ListPositions { get; set; }

        [JsonProperty("listAllPositions")]
        public IList<string> ListAllPositions { get; set; }

        [JsonProperty("misc")]
        public IList<string> Misc { get; set; }

        [JsonProperty("clearScreen")]
        public IList<string> ClearScreen { get; set; }

        [JsonProperty("help")]
        public IList<string> Help { get; set; }

        [JsonProperty("exitApplication")]
        public IList<string> ExitApplication { get; set; }
    }

    public class AppMessages
    {

        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("titleTickerSymbol")]
        public string TitleTickerSymbol { get; set; }

        [JsonProperty("titleTickerSymbolNotSelected")]
        public string TitleTickerSymbolNotSelected { get; set; }

        [JsonProperty("titlePositionSize")]
        public string TitlePositionSize { get; set; }

        [JsonProperty("titleStepSize")]
        public string TitleStepSize { get; set; }

        [JsonProperty("titleLastFormat")]
        public string TitleLastFormat { get; set; }

        [JsonProperty("titleBidAskFormat")]
        public string TitleBidAskFormat { get; set; }

        [JsonProperty("titleVolumeFormat")]
        public string TitleVolumeFormat { get; set; }

        [JsonProperty("titleCloseFormat")]
        public string TitleCloseFormat { get; set; }

        [JsonProperty("titleOpenFormat")]
        public string TitleOpenFormat { get; set; }

        [JsonProperty("titleDivider")]
        public string TitleDivider { get; set; }

        [JsonProperty("menuTitle")]
        public string MenuTitle { get; set; }

        [JsonProperty("menuOptionInvalidEntry")]
        public string MenuOptionInvalidEntry { get; set; }

        [JsonProperty("menuOptionFormat")]
        public string MenuOptionFormat { get; set; }

        [JsonProperty("menuOptionDivider")]
        public string MenuOptionDivider { get; set; }

        [JsonProperty("menuOptionLongestKey")]
        public string MenuOptionLongestKey { get; set; }

        [JsonProperty("menuOptionEntries")]
        public MenuOptionEntries MenuOptionEntries { get; set; }

        [JsonProperty("welcomeMessage")]
        public string WelcomeMessage { get; set; }

        [JsonProperty("multipleAccountsWarningFormat")]
        public string MultipleAccountsWarningFormat { get; set; }

        [JsonProperty("paperAccountPrefix")]
        public string PaperAccountPrefix { get; set; }

        [JsonProperty("accountTypePaper")]
        public string AccountTypePaper { get; set; }

        [JsonProperty("accountTypeLive")]
        public string AccountTypeLive { get; set; }

        [JsonProperty("pressAnyKeyToExit")]
        public string PressAnyKeyToExit { get; set; }

        [JsonProperty("selectTickerPrompt")]
        public string SelectTickerPrompt { get; set; }

        [JsonProperty("stepSizePrompt")]
        public string StepSizePrompt { get; set; }

        [JsonProperty("stepSizeFromCashPrompt")]
        public string StepSizeFromCashPrompt { get; set; }

        [JsonProperty("tickerSymbolSetFormat")]
        public string TickerSymbolSetFormat { get; set; }

        [JsonProperty("tickerSymbolClearedFormat")]
        public string TickerSymbolClearedFormat { get; set; }

        [JsonProperty("stepSizeSetFormat")]
        public string StepSizeSetFormat { get; set; }

        [JsonProperty("loadedStateFormat")]
        public string LoadedStateFormat { get; set; }

        [JsonProperty("savedStateFormat")]
        public string SavedStateFormat { get; set; }

        [JsonProperty("listPositionsFormat")]
        public string ListPositionsFormat { get; set; }

        [JsonProperty("listAllPositionsFormat")]
        public string ListAllPositionsFormat { get; set; }

        [JsonProperty("invalidIntegerInputError")]
        public string InvalidIntegerInputError { get; set; }

        [JsonProperty("invalidDecimalInputError")]
        public string InvalidDecimalInputError { get; set; }

        [JsonProperty("invalidNonEmptyStringInputError")]
        public string InvalidNonEmptyStringInputError { get; set; }

        [JsonProperty("tickerSymbolNotSetError")]
        public string TickerSymbolNotSetError { get; set; }

        [JsonProperty("stepSizeNotSetError")]
        public string StepSizeNotSetError { get; set; }

        [JsonProperty("priceDataUnavailableError")]
        public string PriceDataUnavailableError { get; set; }

        [JsonProperty("positionNotFoundError")]
        public string PositionNotFoundError { get; set; }

        [JsonProperty("twsDisconnectedError")]
        public string TwsDisconnectedError { get; set; }

        [JsonProperty("twsErrorFormat")]
        public string TwsErrorFormat { get; set; }
    }

}
