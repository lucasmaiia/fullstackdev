import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { LeadCard } from '../components/LeadCard'
import type { Lead } from '../types/types';

const base: Lead = {
  id: 1,
  firstName: 'Ana',
  lastName: 'Silva',
  phone: '(31) 9000-0000',
  email: 'ana@mail.com',
  suburb: 'Centro',
  category: 'Plumbing',
  description: 'Kitchen leak',
  price: 650,
  dateCreated: new Date().toISOString(),
  status: 'New'
}

test('renderiza nome, suburb e price', () => {
  render(<LeadCard lead={base} />)
  expect(screen.getByText(/Ana/i)).toBeInTheDocument()
  expect(screen.getByText(/Centro/i)).toBeInTheDocument()
  expect(screen.getByText(/\$650\.00/)).toBeInTheDocument()
})

test('chama onAccept quando clicar em Accept', async () => {
  const onAccept = vi.fn()
  render(<LeadCard lead={base} onAccept={onAccept} />)
  await userEvent.click(screen.getByRole('button', { name: /accept/i }))
  expect(onAccept).toHaveBeenCalledWith(1)
})

test('mostra phone/email quando Accepted', () => {
  const accepted = { ...base, status: 'Accepted' as const }
  render(<LeadCard lead={accepted} />)
  expect(screen.getByText('(31) 9000-0000')).toBeInTheDocument()
  expect(screen.getByText('ana@mail.com')).toBeInTheDocument()
})
