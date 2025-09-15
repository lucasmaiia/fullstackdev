import { setupServer } from 'msw/node'
import { http, HttpResponse } from 'msw'

export const server = setupServer(
  http.get('http://localhost:5080/api/leads', ({ request }) => {
    const url = new URL(request.url)
    const status = url.searchParams.get('status') ?? 'New'
    const data = [
      { id: 1, firstName: 'Ana', lastName: 'Silva', phone: '(31) 9000-0000', email: 'ana@mail.com', suburb: 'Centro', category: 'Plumbing', price: 300, description: 'Kitchen leak', dateCreated: new Date().toISOString(), status },
      { id: 2, firstName: 'Bruno', lastName: 'Souza', phone: '(31) 9111-1111', email: 'bruno@mail.com', suburb: 'Savassi', category: 'Electrical', price: 650, description: 'Outlet repair', dateCreated: new Date().toISOString(), status },
    ]
    return HttpResponse.json(data)
  })
)
