using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Core.Brokers.Binance.Requests
{
	public class SpotOrderRequest
	{
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        [JsonPropertyName("type")]
        public OrderType Type { get; set; }
        [JsonPropertyName("timeStamp")]
        public long TimeStamp { get; set; }
        [JsonPropertyName("timeInForce")]
        public TimeInForce TimeInForce { get; set; }
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("quoteOrderQty")]
        public decimal QuoteOrderQuantity { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("newClientOrderId")]
        public string NewClientOrderID { get; set; }
        [JsonPropertyName("strategyID")]
        public int StrategyID { get; set; }
        [JsonPropertyName("strategyType")]
        public int StrategyType { get; set; }
        [JsonPropertyName("stopPrice")]
        public decimal StopPrice { get; set; }
        [JsonPropertyName("trailingDelta")]
        public long TrailingDelta { get; set; }
        [JsonPropertyName("icebergQty")]
        public decimal IcebergQuantity { get; set; }
        [JsonPropertyName("newOrderRespType")]
        public NewOrderResponseType NewOrderRespType { get; set; }
        [JsonPropertyName("selfTradePreventionMode")]
        public SelfTradePreventionMode SelfTradePreventionMode { get; set; }
        [JsonPropertyName("recvWindow")]
        public long RecvWindow { get; set; }
    }
}

