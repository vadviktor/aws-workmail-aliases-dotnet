/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./**/*.{html,cshtml,razor,js}"],
    theme: {
        extend: {}
    },
    plugins: [
        require('@tailwindcss/forms'),
        //require('@tailwindcss/container-queries')
    ]
}