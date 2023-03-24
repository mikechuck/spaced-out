using Unity.Netcode;
using Unity.Collections;

namespace CustomNetworkVariables
{
	public struct PlayerInfoNetVar : INetworkSerializable, System.IEquatable<PlayerInfoNetVar>
	{
		private ulong _playerId;
		private FixedString128Bytes  _playerName;
		private bool _isReady;
		public ulong PlayerId { get => _playerId; }
		public FixedString128Bytes PlayerName { get => _playerName; }
		public bool IsReady { get => _isReady; }

		public PlayerInfoNetVar(ulong playerId, FixedString128Bytes  playerName)
		{
			_playerId = playerId;
			_playerName = playerName;
			_isReady = false;
		}
		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			if (serializer.IsReader)
			{
				var reader = serializer.GetFastBufferReader();
				reader.ReadValueSafe(out _playerId);
				reader.ReadValueSafe(out _playerName);
				reader.ReadValueSafe(out _isReady);
			}
			else
			{
				var writer = serializer.GetFastBufferWriter();
				writer.WriteValueSafe(_playerId);
				writer.WriteValueSafe(_playerName);
				writer.WriteValueSafe(_isReady);
			}
		}

		public bool Equals(PlayerInfoNetVar other)
		{
			return other.Equals(this);
		}
	}
}