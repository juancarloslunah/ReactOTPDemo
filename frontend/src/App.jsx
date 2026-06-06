import { useEffect, useState } from 'react'
import axios from 'axios'

function App() {

  const [otp, setOtp] = useState('')
  const [secret, setSecret] = useState('')
  const [qrCode, setQrCode] = useState('')
  const [resultado, setResultado] = useState('')
  const [loading, setLoading] = useState(false)

  useEffect(() => {

    cargarQR()

  }, [])

  const cargarQR = async () => {

    try {

      const response = await axios.get(
        'https://reactotpdemo.onrender.com/api/otp/generate'
      )

      setSecret(response.data.secret)
      setQrCode(response.data.qrCode)

    }
    catch (error) {

      console.error(error)

      setResultado('Error cargando QR')

    }
  }

  const validarOTP = async () => {

    if (!otp) {

      setResultado('Debe ingresar un OTP')
      return
    }

    try {

      setLoading(true)

      const response = await axios.post(
        'https://reactotpdemo.onrender.com/api/otp/validate',
        {
          secret: secret,
          otp: otp
        }
      )

      setResultado(response.data.message)

    }
    catch (error) {

      console.error(error)

      setResultado('Error al conectar con la API')

    }
    finally {

      setLoading(false)
    }
  }

  return (

    <div
      style={{
        padding: '40px',
        fontFamily: 'Arial',
        textAlign: 'center'
      }}
    >

      <h1>Portal Validación OTP</h1>

      <p>
        Escanee el código QR con Microsoft Authenticator
      </p>

      {
        qrCode &&
        <img
          src={qrCode}
          alt="QR OTP"
          width="300"
        />
      }

      <br />
      <br />

      <strong>Secret:</strong>

      <br />

      <span>{secret}</span>

      <br />
      <br />

      <input
        type="text"
        maxLength="6"
        placeholder="Ingrese OTP"
        value={otp}
        onChange={(e) => setOtp(e.target.value)}
        style={{
          padding: '10px',
          width: '200px',
          textAlign: 'center',
          fontSize: '18px'
        }}
      />

      <br />
      <br />

      <button
        onClick={validarOTP}
        style={{
          padding: '10px',
          width: '220px',
          cursor: 'pointer'
        }}
      >
        Validar OTP
      </button>

      <br />
      <br />

      {
        loading &&
        <p>Validando...</p>
      }

      <h3>Resultado:</h3>

      <strong
        style={{
          color:
            resultado === 'OTP válido'
              ? 'green'
              : 'red'
        }}
      >
        {resultado}
      </strong>

    </div>
  )
}

export default App