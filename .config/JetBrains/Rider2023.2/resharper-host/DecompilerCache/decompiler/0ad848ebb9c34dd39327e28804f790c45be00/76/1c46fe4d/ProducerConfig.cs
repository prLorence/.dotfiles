// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ProducerConfig
// Assembly: Confluent.Kafka, Version=2.2.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: 0AD848EB-B9C3-4DD3-9327-E28804F790C4
// Assembly location: /home/phetoush/.nuget/packages/confluent.kafka/2.2.0/lib/net6.0/Confluent.Kafka.dll
// XML documentation location: /home/phetoush/.nuget/packages/confluent.kafka/2.2.0/lib/net6.0/Confluent.Kafka.xml

using System.Collections.Generic;

namespace Confluent.Kafka
{
  /// <summary>Producer configuration properties</summary>
  public class ProducerConfig : ClientConfig
  {
    /// <summary>
    ///     Initialize a new empty <see cref="T:Confluent.Kafka.ProducerConfig" /> instance.
    /// </summary>
    public ProducerConfig()
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ProducerConfig" /> instance wrapping
    ///     an existing <see cref="T:Confluent.Kafka.ClientConfig" /> instance.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ProducerConfig(ClientConfig config)
      : base(config)
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ProducerConfig" /> instance wrapping
    ///     an existing key/value pair collection.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ProducerConfig(IDictionary<string, string> config)
      : base(config)
    {
    }

    /// <summary>
    ///     Check if any properties have been set that have implications for
    ///     application logic and therefore shouldn't be set via external
    ///     configuration, independent of the code. Throw an ArgumentException
    ///     if so.
    /// 
    ///     There are currently no such Producer configuration properties
    ///     and this method will never throw.
    /// </summary>
    public ProducerConfig ThrowIfContainsNonUserConfigurable() => this;

    /// <summary>
    ///     Specifies whether or not the producer should start a background poll
    ///     thread to receive delivery reports and event notifications. Generally,
    ///     this should be set to true. If set to false, you will need to call
    ///     the Poll function manually.
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? EnableBackgroundPoll
    {
      get => this.GetBool("dotnet.producer.enable.background.poll");
      set => this.SetObject("dotnet.producer.enable.background.poll", (object) value);
    }

    /// <summary>
    ///     Specifies whether to enable notification of delivery reports. Typically
    ///     you should set this parameter to true. Set it to false for "fire and
    ///     forget" semantics and a small boost in performance.
    /// 
    ///     default: true
    ///     importance: low
    /// </summary>
    public bool? EnableDeliveryReports
    {
      get => this.GetBool("dotnet.producer.enable.delivery.reports");
      set => this.SetObject("dotnet.producer.enable.delivery.reports", (object) value);
    }

    /// <summary>
    ///     A comma separated list of fields that may be optionally set in delivery
    ///     reports. Disabling delivery report fields that you do not require will
    ///     improve maximum throughput and reduce memory usage. Allowed values:
    ///     key, value, timestamp, headers, status, all, none.
    /// 
    ///     default: all
    ///     importance: low
    /// </summary>
    public string DeliveryReportFields
    {
      get => this.Get("dotnet.producer.delivery.report.fields");
      set => this.SetObject("dotnet.producer.delivery.report.fields", (object) value.ToString());
    }

    /// <summary>
    ///     The ack timeout of the producer request in milliseconds. This value is only enforced by the broker and relies on `request.required.acks` being != 0.
    /// 
    ///     default: 30000
    ///     importance: medium
    /// </summary>
    public int? RequestTimeoutMs
    {
      get => this.GetInt("request.timeout.ms");
      set => this.SetObject("request.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Local message timeout. This value is only enforced locally and limits the time a produced message waits for successful delivery. A time of 0 is infinite. This is the maximum time librdkafka may use to deliver a message (including retries). Delivery error occurs when either the retry count or the message timeout are exceeded. The message timeout is automatically adjusted to `transaction.timeout.ms` if `transactional.id` is configured.
    /// 
    ///     default: 300000
    ///     importance: high
    /// </summary>
    public int? MessageTimeoutMs
    {
      get => this.GetInt("message.timeout.ms");
      set => this.SetObject("message.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Partitioner: `random` - random distribution, `consistent` - CRC32 hash of key (Empty and NULL keys are mapped to single partition), `consistent_random` - CRC32 hash of key (Empty and NULL keys are randomly partitioned), `murmur2` - Java Producer compatible Murmur2 hash of key (NULL keys are mapped to single partition), `murmur2_random` - Java Producer compatible Murmur2 hash of key (NULL keys are randomly partitioned. This is functionally equivalent to the default partitioner in the Java Producer.), `fnv1a` - FNV-1a hash of key (NULL keys are mapped to single partition), `fnv1a_random` - FNV-1a hash of key (NULL keys are randomly partitioned).
    /// 
    ///     default: consistent_random
    ///     importance: high
    /// </summary>
    public Confluent.Kafka.Partitioner? Partitioner
    {
      get => (Confluent.Kafka.Partitioner?) this.GetEnum(typeof (Confluent.Kafka.Partitioner), "partitioner");
      set => this.SetObject("partitioner", (object) value);
    }

    /// <summary>
    ///     Compression level parameter for algorithm selected by configuration property `compression.codec`. Higher values will result in better compression at the cost of more CPU usage. Usable range is algorithm-dependent: [0-9] for gzip; [0-12] for lz4; only 0 for snappy; -1 = codec-dependent default compression level.
    /// 
    ///     default: -1
    ///     importance: medium
    /// </summary>
    public int? CompressionLevel
    {
      get => this.GetInt("compression.level");
      set => this.SetObject("compression.level", (object) value);
    }

    /// <summary>
    ///     Enables the transactional producer. The transactional.id is used to identify the same transactional producer instance across process restarts. It allows the producer to guarantee that transactions corresponding to earlier instances of the same producer have been finalized prior to starting any new transactions, and that any zombie instances are fenced off. If no transactional.id is provided, then the producer is limited to idempotent delivery (if enable.idempotence is set). Requires broker version &gt;= 0.11.0.
    /// 
    ///     default: ''
    ///     importance: high
    /// </summary>
    public string TransactionalId
    {
      get => this.Get("transactional.id");
      set => this.SetObject("transactional.id", (object) value);
    }

    /// <summary>
    ///     The maximum amount of time in milliseconds that the transaction coordinator will wait for a transaction status update from the producer before proactively aborting the ongoing transaction. If this value is larger than the `transaction.max.timeout.ms` setting in the broker, the init_transactions() call will fail with ERR_INVALID_TRANSACTION_TIMEOUT. The transaction timeout automatically adjusts `message.timeout.ms` and `socket.timeout.ms`, unless explicitly configured in which case they must not exceed the transaction timeout (`socket.timeout.ms` must be at least 100ms lower than `transaction.timeout.ms`). This is also the default timeout value if no timeout (-1) is supplied to the transactional API methods.
    /// 
    ///     default: 60000
    ///     importance: medium
    /// </summary>
    public int? TransactionTimeoutMs
    {
      get => this.GetInt("transaction.timeout.ms");
      set => this.SetObject("transaction.timeout.ms", (object) value);
    }

    /// <summary>
    ///     When set to `true`, the producer will ensure that messages are successfully produced exactly once and in the original produce order. The following configuration properties are adjusted automatically (if not modified by the user) when idempotence is enabled: `max.in.flight.requests.per.connection=5` (must be less than or equal to 5), `retries=INT32_MAX` (must be greater than 0), `acks=all`, `queuing.strategy=fifo`. Producer instantation will fail if user-supplied configuration is incompatible.
    /// 
    ///     default: false
    ///     importance: high
    /// </summary>
    public bool? EnableIdempotence
    {
      get => this.GetBool("enable.idempotence");
      set => this.SetObject("enable.idempotence", (object) value);
    }

    /// <summary>
    ///     **EXPERIMENTAL**: subject to change or removal. When set to `true`, any error that could result in a gap in the produced message series when a batch of messages fails, will raise a fatal error (ERR__GAPLESS_GUARANTEE) and stop the producer. Messages failing due to `message.timeout.ms` are not covered by this guarantee. Requires `enable.idempotence=true`.
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? EnableGaplessGuarantee
    {
      get => this.GetBool("enable.gapless.guarantee");
      set => this.SetObject("enable.gapless.guarantee", (object) value);
    }

    /// <summary>
    ///     Maximum number of messages allowed on the producer queue. This queue is shared by all topics and partitions. A value of 0 disables this limit.
    /// 
    ///     default: 100000
    ///     importance: high
    /// </summary>
    public int? QueueBufferingMaxMessages
    {
      get => this.GetInt("queue.buffering.max.messages");
      set => this.SetObject("queue.buffering.max.messages", (object) value);
    }

    /// <summary>
    ///     Maximum total message size sum allowed on the producer queue. This queue is shared by all topics and partitions. This property has higher priority than queue.buffering.max.messages.
    /// 
    ///     default: 1048576
    ///     importance: high
    /// </summary>
    public int? QueueBufferingMaxKbytes
    {
      get => this.GetInt("queue.buffering.max.kbytes");
      set => this.SetObject("queue.buffering.max.kbytes", (object) value);
    }

    /// <summary>
    ///     Delay in milliseconds to wait for messages in the producer queue to accumulate before constructing message batches (MessageSets) to transmit to brokers. A higher value allows larger and more effective (less overhead, improved compression) batches of messages to accumulate at the expense of increased message delivery latency.
    /// 
    ///     default: 5
    ///     importance: high
    /// </summary>
    public double? LingerMs
    {
      get => this.GetDouble("linger.ms");
      set => this.SetObject("linger.ms", (object) value);
    }

    /// <summary>
    ///     How many times to retry sending a failing Message. **Note:** retrying may cause reordering unless `enable.idempotence` is set to true.
    /// 
    ///     default: 2147483647
    ///     importance: high
    /// </summary>
    public int? MessageSendMaxRetries
    {
      get => this.GetInt("message.send.max.retries");
      set => this.SetObject("message.send.max.retries", (object) value);
    }

    /// <summary>
    ///     The backoff time in milliseconds before retrying a protocol request.
    /// 
    ///     default: 100
    ///     importance: medium
    /// </summary>
    public int? RetryBackoffMs
    {
      get => this.GetInt("retry.backoff.ms");
      set => this.SetObject("retry.backoff.ms", (object) value);
    }

    /// <summary>
    ///     The threshold of outstanding not yet transmitted broker requests needed to backpressure the producer's message accumulator. If the number of not yet transmitted requests equals or exceeds this number, produce request creation that would have otherwise been triggered (for example, in accordance with linger.ms) will be delayed. A lower number yields larger and more effective batches. A higher value can improve latency when using compression on slow machines.
    /// 
    ///     default: 1
    ///     importance: low
    /// </summary>
    public int? QueueBufferingBackpressureThreshold
    {
      get => this.GetInt("queue.buffering.backpressure.threshold");
      set => this.SetObject("queue.buffering.backpressure.threshold", (object) value);
    }

    /// <summary>
    ///     compression codec to use for compressing message sets. This is the default value for all topics, may be overridden by the topic configuration property `compression.codec`.
    /// 
    ///     default: none
    ///     importance: medium
    /// </summary>
    public Confluent.Kafka.CompressionType? CompressionType
    {
      get => (Confluent.Kafka.CompressionType?) this.GetEnum(typeof (Confluent.Kafka.CompressionType), "compression.type");
      set => this.SetObject("compression.type", (object) value);
    }

    /// <summary>
    ///     Maximum number of messages batched in one MessageSet. The total MessageSet size is also limited by batch.size and message.max.bytes.
    /// 
    ///     default: 10000
    ///     importance: medium
    /// </summary>
    public int? BatchNumMessages
    {
      get => this.GetInt("batch.num.messages");
      set => this.SetObject("batch.num.messages", (object) value);
    }

    /// <summary>
    ///     Maximum size (in bytes) of all messages batched in one MessageSet, including protocol framing overhead. This limit is applied after the first message has been added to the batch, regardless of the first message's size, this is to ensure that messages that exceed batch.size are produced. The total MessageSet size is also limited by batch.num.messages and message.max.bytes.
    /// 
    ///     default: 1000000
    ///     importance: medium
    /// </summary>
    public int? BatchSize
    {
      get => this.GetInt("batch.size");
      set => this.SetObject("batch.size", (object) value);
    }

    /// <summary>
    ///     Delay in milliseconds to wait to assign new sticky partitions for each topic. By default, set to double the time of linger.ms. To disable sticky behavior, set to 0. This behavior affects messages with the key NULL in all cases, and messages with key lengths of zero when the consistent_random partitioner is in use. These messages would otherwise be assigned randomly. A higher value allows for more effective batching of these messages.
    /// 
    ///     default: 10
    ///     importance: low
    /// </summary>
    public int? StickyPartitioningLingerMs
    {
      get => this.GetInt("sticky.partitioning.linger.ms");
      set => this.SetObject("sticky.partitioning.linger.ms", (object) value);
    }
  }
}
