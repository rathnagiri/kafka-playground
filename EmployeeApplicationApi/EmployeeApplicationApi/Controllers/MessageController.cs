using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApplicationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController(ILogger<MessageController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> PublishRequestMessage(string sportId, string groupId)
        {
            // Kafka producer configuration
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "kafka:9092",
                Acks = Acks.All
            };

            // Construct the message payload
            var payload = new MessagePayload
            {
                SportId = sportId,
                GroupId = groupId
            };

            var messageValue = JsonSerializer.Serialize(payload);

            // Kafka topic
            const string topic = "Automation.Pricing.Request";

            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            {
                try
                {
                    // Produce the message
                    var deliveryResult = await producer.ProduceAsync(topic, new Message<string, string>
                    {
                        Key = sportId, 
                        Value = messageValue
                    });

                    // Log the result
                    logger.LogInformation($"Message delivered to Topic: {deliveryResult.Topic} | Partition: {deliveryResult.TopicPartition} | Offset: {deliveryResult.Offset}");

                    // Return success response
                    return Ok($"Message with SportId: {sportId} and GroupId: {groupId} has been successfully published to topic: {topic}.");
                }
                catch (ProduceException<string, string> e)
                {
                    // Log the error
                    logger.LogError($"Message delivery failed: {e.Error.Reason}");
                    return StatusCode(500, $"Failed to publish message: {e.Error.Reason}");
                }
            }
        }

    }
}
