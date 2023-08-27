// Type: Confluent.Kafka.IProducer`2 
// Assembly: Confluent.Kafka, Version=2.2.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: 0AD848EB-B9C3-4DD3-9327-E28804F790C4
// Location: /home/phetoush/.nuget/packages/confluent.kafka/2.2.0/lib/net6.0/Confluent.Kafka.dll
// Sequence point data and variable names from decompiler

.class interface public abstract auto ansi
  Confluent.Kafka.IProducer`2<TKey, TValue>
    implements
      Confluent.Kafka.IClient,
      [System.Runtime]System.IDisposable
{

  .method public hidebysig virtual newslot abstract instance class [System.Runtime]System.Threading.Tasks.Task`1<class Confluent.Kafka.DeliveryResult`2<!0/*TKey*/, !1/*TValue*/>>
    ProduceAsync(
      string topic,
      class Confluent.Kafka.Message`2<!0/*TKey*/, !1/*TValue*/> message,
      [opt] valuetype [System.Runtime]System.Threading.CancellationToken cancellationToken
    ) cil managed
  {
    .param [3] = nullref
    // Can't find a body
  } // end of method IProducer`2::ProduceAsync

  .method public hidebysig virtual newslot abstract instance class [System.Runtime]System.Threading.Tasks.Task`1<class Confluent.Kafka.DeliveryResult`2<!0/*TKey*/, !1/*TValue*/>>
    ProduceAsync(
      class Confluent.Kafka.TopicPartition topicPartition,
      class Confluent.Kafka.Message`2<!0/*TKey*/, !1/*TValue*/> message,
      [opt] valuetype [System.Runtime]System.Threading.CancellationToken cancellationToken
    ) cil managed
  {
    .param [3] = nullref
    // Can't find a body
  } // end of method IProducer`2::ProduceAsync

  .method public hidebysig virtual newslot abstract instance void
    Produce(
      string topic,
      class Confluent.Kafka.Message`2<!0/*TKey*/, !1/*TValue*/> message,
      [opt] class [System.Runtime]System.Action`1<class Confluent.Kafka.DeliveryReport`2<!0/*TKey*/, !1/*TValue*/>> deliveryHandler
    ) cil managed
  {
    .param [3] = nullref
    // Can't find a body
  } // end of method IProducer`2::Produce

  .method public hidebysig virtual newslot abstract instance void
    Produce(
      class Confluent.Kafka.TopicPartition topicPartition,
      class Confluent.Kafka.Message`2<!0/*TKey*/, !1/*TValue*/> message,
      [opt] class [System.Runtime]System.Action`1<class Confluent.Kafka.DeliveryReport`2<!0/*TKey*/, !1/*TValue*/>> deliveryHandler
    ) cil managed
  {
    .param [3] = nullref
    // Can't find a body
  } // end of method IProducer`2::Produce

  .method public hidebysig virtual newslot abstract instance int32
    Poll(
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::Poll

  .method public hidebysig virtual newslot abstract instance int32
    Flush(
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::Flush

  .method public hidebysig virtual newslot abstract instance void
    Flush(
      [opt] valuetype [System.Runtime]System.Threading.CancellationToken cancellationToken
    ) cil managed
  {
    .param [1] = nullref
    // Can't find a body
  } // end of method IProducer`2::Flush

  .method public hidebysig virtual newslot abstract instance void
    InitTransactions(
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::InitTransactions

  .method public hidebysig virtual newslot abstract instance void
    BeginTransaction() cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::BeginTransaction

  .method public hidebysig virtual newslot abstract instance void
    CommitTransaction(
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::CommitTransaction

  .method public hidebysig virtual newslot abstract instance void
    CommitTransaction() cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::CommitTransaction

  .method public hidebysig virtual newslot abstract instance void
    AbortTransaction(
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::AbortTransaction

  .method public hidebysig virtual newslot abstract instance void
    AbortTransaction() cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::AbortTransaction

  .method public hidebysig virtual newslot abstract instance void
    SendOffsetsToTransaction(
      class [System.Runtime]System.Collections.Generic.IEnumerable`1<class Confluent.Kafka.TopicPartitionOffset> offsets,
      class Confluent.Kafka.IConsumerGroupMetadata groupMetadata,
      valuetype [System.Runtime]System.TimeSpan timeout
    ) cil managed
  {
    // Can't find a body
  } // end of method IProducer`2::SendOffsetsToTransaction
} // end of class Confluent.Kafka.IProducer`2
