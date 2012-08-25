﻿using System;

namespace ACBr.Net
{
    public sealed class ACBrBal : ACBrComponent, IDisposable
    {
        #region Constructor

        public ACBrBal()
        {
			Create(ACBrDll.BAL_Create);
        }

        #endregion Constructor

        #region Properties

        public bool Ativo
        {
            get
            {
                return GetBool(ACBrDll.BAL_GetAtivo);
            }
        }

        public ModeloBal Modelo
        {
            get
            {
                return (ModeloBal)GetInt32(ACBrDll.BAL_GetModelo);
            }
            set
            {
                SetInt32(ACBrDll.BAL_SetModelo, (int)value);
            }
        }

        public string ModeloStr
        {
            get
            {
                return GetString(ACBrDll.BAL_GetModeloStr);
            }
        }

        public string Porta
        {
            get
            {
                return GetString(ACBrDll.BAL_GetPorta);
            }
            set
            {
                SetString(ACBrDll.BAL_SetPorta, value);
            }
        }

        public decimal UltimoPesoLido
        {
            get
            {
                return GetDecimal(ACBrDll.BAL_GetUltimoPesoLido);
            }
        }

        public string UltimaResposta
        {
            get
            {
                return GetString(ACBrDll.BAL_GetUltimaResposta);
            }
        }

        #endregion Properties

        #region Methods

        public void Ativar()
        {
            int ret = ACBrDll.BAL_Ativar(this.Handle);
            CheckResult(ret);
        }

        public void Desativar()
        {
            int ret = ACBrDll.BAL_Desativar(this.Handle);
            CheckResult(ret);
        }

        public decimal LePeso()
        {
            const int DEFAULT_TIMEOUT = 3000;
            return LePeso(DEFAULT_TIMEOUT);
        }

        public decimal LePeso(int timeout)
        {
            double peso = 0;
            int ret = ACBrDll.BAL_LePeso(this.Handle, timeout, ref peso);
            CheckResult(ret);

            return Convert.ToDecimal(peso);
        }

        #region Override Methods

        protected internal override void CheckResult(int ret)
        {
            switch (ret)
            {
                case -1:

                    string error = GetString(ACBrDll.BAL_GetUltimoErro);
                    throw new ACBrECFException(error);

                case -2:

                    throw new ACBrECFException("ACBr BAL não inicializado.");
            }
        }

        protected override void OnDisposing()
        {
            if (this.Handle != IntPtr.Zero)
            {
				Destroy(ACBrDll.BAL_Destroy);
            }
        }

        #endregion Override Methods

        #endregion Methods
    }
}