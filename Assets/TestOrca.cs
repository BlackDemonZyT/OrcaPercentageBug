using Orca;
using Solana.Unity.Dex.Math;
using Solana.Unity.Dex.Models;
using Solana.Unity.Dex;
using Solana.Unity.Rpc.Types;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOrca : MonoBehaviour
{
    public Web3 web3object;

    public WalletBase walletCurrent;
    public Account accountCurrent;

    public async void OnClickConnectWallet()
    {
        accountCurrent = await web3object.LoginWalletAdapter();
        walletCurrent = web3object.WalletBase;

        string publicKey = accountCurrent.PublicKey;
        Debug.Log("PUBLIC KEY: " + publicKey);

        IDex dex = new OrcaDex(Web3.Account, Web3.Rpc, commitment: Commitment.Confirmed);
        TokenData tokenA = await dex.GetTokenBySymbol("USDC");
        TokenData tokenB = await dex.GetTokenBySymbol("KING");

        Pool _whirlpool = await dex.FindWhirlpoolAddress(tokenA.MintAddress, tokenB.MintAddress);

        var converted = DecimalUtil.ToUlong(2.0, tokenA.Decimals);

        var swapQuote = await dex
            .GetSwapQuoteFromWhirlpool(_whirlpool.Address,
            converted,
            tokenA.MintAddress);

        var quote = DecimalUtil.FromBigInteger(swapQuote.EstimatedAmountOut, tokenB.Decimals);
        Debug.Log(quote);
    }
}
