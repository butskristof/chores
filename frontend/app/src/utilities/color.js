// pass in a color in hex format ("#00ff00"), get back the color which text on top of it
// should be (white or black)
export const getTextColorForBackground = (color) => {
  const r = parseInt(color.substring(1, 3), 16);
  const g = parseInt(color.substring(3, 5), 16);
  const b = parseInt(color.substring(5, 7), 16);
  // http://www.w3.org/TR/AERT#color-contrast
  const yiq = (r * 299 + g * 587 + b * 114) / 1000;
  return yiq >= 128 ? 'var(--p-surface-700)' : 'white';
};
