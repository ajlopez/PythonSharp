class Atom:
  "Atom class"
  def __init__(self, symbol, mass, position):
    self.symbol = symbol
    self.mass = mass
    self.position = position

  def getSymbol(self):
    return self.symbol

  def getMass(self):
    return self.mass

  def getPosition(self):
    return self.position

oAtom = Atom('O', 15.9994, [0.0, 0.0, 0.0])
hAtom1 = Atom('H', 1.0079, [0.0, 1.0, 0.0])
hAtom2 = Atom('H', 1.0079, [1.0, 0.0, 0.0])

print('The mass of the second H atom is ', hAtom2.getPosition())