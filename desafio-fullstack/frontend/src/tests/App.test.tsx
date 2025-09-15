import { render, screen, waitFor } from '@testing-library/react'
import App from '../App'

test('carrega e lista leads', async () => {
  render(<App />)

  // título
  expect(screen.getByRole('heading', { name: /Lead Manager/i })).toBeInTheDocument()

  // itens vindos do MSW
  await waitFor(() => {
    expect(screen.getByText(/Kitchen leak/i)).toBeInTheDocument()
    expect(screen.getByText(/Outlet repair/i)).toBeInTheDocument()
  })
})
