﻿using System;

namespace WalletSystems.Responses.Dtos
{
    public class WalletDto
    {
        public WalletDto(Guid id, decimal balance)
        {
            Id = id;
            Balance = balance;
        }

        public Guid Id { get; }
        public decimal Balance { get; }
    }
}