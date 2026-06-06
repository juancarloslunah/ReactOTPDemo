import { useState } from 'react'
import axios from 'axios'

function App() {

  const [otp, setOtp] = useState('')
  const [resultado, setResultado] = useState('')
  const [loading, setLoading] = useState(false)

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
        fontFamily: 'Arial'
      }}
    >

      <h1>Demo OTP - Jarvis</h1>

      <p>
        Validación OTP con Microsoft Authenticator
      </p>

      <input
        type="text"
        maxLength="6"
        placeholder="Ingrese OTP"
        value={otp}
        onChange={(e) => setOtp(e.target.value)}
        style={{
          padding: '10px',
          width: '200px'
        }}
      />

      <br />
      <br />

      <button
        onClick={validarOTP}
        style={{
          padding: '10px',
          width: '220px'
        }}
      >
        Validar OTP
      </button>

      <br />
      <br />

      {
        loading
          ? <p>Validando...</p>
          : null
      }

      <h3>Resultado:</h3>

      <strong>{resultado}</strong>

    </div>
  )
}

export default App