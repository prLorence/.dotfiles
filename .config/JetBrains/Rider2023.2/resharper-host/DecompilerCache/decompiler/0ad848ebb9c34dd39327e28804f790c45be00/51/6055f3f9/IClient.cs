// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.IClient
// Assembly: Confluent.Kafka, Version=2.2.0.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: 0AD848EB-B9C3-4DD3-9327-E28804F790C4
// Assembly location: /home/phetoush/.nuget/packages/confluent.kafka/2.2.0/lib/net6.0/Confluent.Kafka.dll
// XML documentation location: /home/phetoush/.nuget/packages/confluent.kafka/2.2.0/lib/net6.0/Confluent.Kafka.xml

using System;

namespace Confluent.Kafka
{
  /// <summary>Defines methods common to all client types.</summary>
  public interface IClient : IDisposable
  {
    /// <summary>
    ///     An opaque reference to the underlying
    ///     librdkafka client instance. This can be used
    ///     to construct an AdminClient that utilizes the
    ///     same underlying librdkafka client as this
    ///     instance.
    /// </summary>
    Handle Handle { get; }

    /// <summary>
    ///     Gets the name of this client instance.
    /// 
    ///     Contains (but is not equal to) the client.id
    ///     configuration parameter.
    /// </summary>
    /// <remarks>
    ///     This name will be unique across all client
    ///     instances in a given application which allows
    ///     log messages to be associated with the
    ///     corresponding instance.
    /// </remarks>
    string Name { get; }

    /// <summary>
    ///     Adds one or more brokers to the Client's list
    ///     of initial bootstrap brokers.
    /// 
    ///     Note: Additional brokers are discovered
    ///     automatically as soon as the Client connects
    ///     to any broker by querying the broker metadata.
    ///     Calling this method is only required in some
    ///     scenarios where the address of all brokers in
    ///     the cluster changes.
    /// </summary>
    /// <param name="brokers">
    ///     Comma-separated list of brokers in
    ///     the same format as the bootstrap.server
    ///     configuration parameter.
    /// </param>
    /// <remarks>
    ///     There is currently no API to remove existing
    ///     configured, added or learnt brokers.
    /// </remarks>
    /// <returns>
    ///     The number of brokers added. This value
    ///     includes brokers that may have been specified
    ///     a second time.
    /// </returns>
    int AddBrokers(string brokers);

    /// <summary>
    ///     SetSaslCredentials sets the SASL credentials used for this
    ///     client.
    ///     The new credentials will overwrite the old ones (which were set
    ///     when creating client or by a previous call to
    ///     SetSaslCredentials). The new credentials will be used the next
    ///     time the client needs to authenticate to a broker.
    ///     This method will not disconnect existing broker connections that
    ///     were established with the old credentials.
    ///     This method applies only to the SASL PLAIN and SCRAM mechanisms.
    /// </summary>
    /// <param name="username">The username to set.</param>
    /// <param name="password">The password to set.</param>
    void SetSaslCredentials(string username, string password);
  }
}
