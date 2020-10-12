using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderTracker.Models
{
	public class Order
	{
		public int Number { get; set; }
		public List<OrderHistoryEntry> History;

		protected OrderStatus CurrentStatus
		{
			get => History[History.Count - 1].Status;
		}

		public Order()
		{
			History = new List<OrderHistoryEntry>();

			//set initial status
			StatusPicking(null);
		}

		public void ChangeStatus(OrderStatus status, string comment)
		{
			switch(status)
			{
				case OrderStatus.Picking:
					StatusPicking(comment);
					break;
				case OrderStatus.ReadyToSend:
					StatusReadyToSend(comment);
					break;
				case OrderStatus.Sent:
					StatusSent(comment);
					break;
				case OrderStatus.InTransit:
					StatusInTransit(comment);
					break;
				case OrderStatus.ReadyToPickUp:
					StatusReadyToPickup(comment);
					break;
				case OrderStatus.PickedUp:
					StatusPickedUp(comment);
					break;
				case OrderStatus.Rejected:
					StatusRejected(comment);
					break;
				case OrderStatus.Cancelled:
					StatusCancelled(comment);
					break;
			}
		}

		private void StatusPicking(string comment)
		{
			//can change back from ReadyToSend to Picking
			if (History.Count == 0 || CurrentStatus == OrderStatus.ReadyToSend)
				SetStatus(OrderStatus.Picking, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusReadyToSend(string comment)
		{
			//can change to ReadyToSend only from Picking
			if (CurrentStatus == OrderStatus.Picking)
				SetStatus(OrderStatus.ReadyToSend, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusSent(string comment)
		{
			//can change to Sent only from ReadyToSend
			if (CurrentStatus == OrderStatus.ReadyToSend)
				SetStatus(OrderStatus.Sent, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusInTransit(string comment)
		{
			//can change to InTransit only from ReadyToSend or any other InTransit status
			if (CurrentStatus == OrderStatus.Sent || CurrentStatus == OrderStatus.InTransit)
				SetStatus(OrderStatus.InTransit, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusReadyToPickup(string comment)
		{
			//can change to ReadyToPickup only from InTransit
			if (CurrentStatus == OrderStatus.InTransit)
				SetStatus(OrderStatus.ReadyToPickUp, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusPickedUp(string comment)
		{
			//can change to PickedUp only from ReadyToPickUp
			if (CurrentStatus == OrderStatus.ReadyToPickUp)
				SetStatus(OrderStatus.PickedUp, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusRejected(string comment)
		{
			//can change to Rejected only from ReadyToPickUp
			if (CurrentStatus == OrderStatus.ReadyToPickUp)
				SetStatus(OrderStatus.Rejected, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void StatusCancelled(string comment)
		{
			//can change to Cancelled from any status except finished one
			if (CurrentStatus != OrderStatus.Cancelled &&
				CurrentStatus != OrderStatus.Rejected &&
				CurrentStatus != OrderStatus.PickedUp)
				SetStatus(OrderStatus.Cancelled, comment);
			else
				throw new InvalidOperationException("Can not change status");
		}

		private void SetStatus(OrderStatus status, string comment)
		{
			//here could be description for each status in native language
			//using ToString() for simplicity
			if (string.IsNullOrEmpty(comment))
				comment = status.ToString();

			History.Add(new OrderHistoryEntry()
			{
				Timestamp = DateTime.Now,
				Status = status,
				StatusComment = comment
			});

		}
	}


	public class OrderHistoryEntry
	{
		public DateTime Timestamp { get; set; }
		public OrderStatus Status { get; set; }
		public string StatusComment { get; set; }
	}


	public enum OrderStatus
	{
		Picking = 1,
		ReadyToSend = 2,
		Sent = 3,
		InTransit = 4,
		ReadyToPickUp = 5,
		PickedUp = 6,
		Rejected = 7,
		Cancelled = 8
	}
}
