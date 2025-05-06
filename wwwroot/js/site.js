// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Initialize AOS
AOS.init({ duration: 800, once: true });

// — Back to Top Button —
const backBtn = document.getElementById('backToTop');
window.addEventListener('scroll', () => {
    backBtn.style.display = window.scrollY > 300 ? 'block' : 'none';
});
backBtn.addEventListener('click', () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
});

// — Burger Menu Toggle —
const menuToggle = document.getElementById('menuToggle');
const overlay = document.getElementById('overlayMenu');
const navbar = document.querySelector('.navbar');
const bodyEl = document.body;
const overlayLinks = document.querySelectorAll('#overlayMenu a');

// Toggle overlay menu on burger click
menuToggle.addEventListener('click', () => {
    const icon = menuToggle.querySelector('i');
    const isOpening = !overlay.classList.contains('show');

    overlay.classList.toggle('show');
    overlay.classList.toggle('solid', isOpening);
    icon.classList.toggle('fa-bars', !isOpening);
    icon.classList.toggle('fa-times', isOpening);

    bodyEl.classList.toggle('no-scroll', isOpening);
    navbar.classList.toggle('menu-open', isOpening);
});

// Close overlay when any link is clicked
overlayLinks.forEach(link => {
    link.addEventListener('click', () => {
        overlay.classList.remove('show', 'solid');
        bodyEl.classList.remove('no-scroll');
        navbar.classList.remove('menu-open');

        const icon = menuToggle.querySelector('i');
        icon.classList.add('fa-bars');
        icon.classList.remove('fa-times');
    });
});


// Accessibility Panel Toggle
const accBtn = document.getElementById('accessibilityBtn');
const accPanel = document.getElementById('accessibilityPanel');

// Toggle panel
accBtn.addEventListener('click', (e) => {
    e.stopPropagation();
    accPanel.classList.toggle('active');
});

// Close panel when clicking outside
document.addEventListener('click', (e) => {
    if (!accPanel.contains(e.target) && !accBtn.contains(e.target)) {
        accPanel.classList.remove('active');
    }
});

// Prevent panel clicks from closing it
accPanel.addEventListener('click', (e) => {
    e.stopPropagation();
});

// Feature logic with 4-state text size cycling
let textSizeState = 0; // 0 = default (100%), 1 = 110%, 2 = 120%, 3 = 130%

document.querySelectorAll('.accessibility-option').forEach(btn => {
    btn.addEventListener('click', () => {
        const feature = btn.dataset.feature;
        const main = document.getElementById('mainContent');

        if (feature !== 'textEnlarge') {
            btn.classList.toggle('active');
        }

        switch (feature) {
            case 'textEnlarge':
                const bodyEl = document.body;
                // Remove all text size classes
                document.body.classList.remove('text-size-1', 'text-size-2', 'text-size-3');

                // Cycle through 4 states (0-3)
                textSizeState = (textSizeState + 1) % 4;

                // Apply new size class if not state 0
                if (textSizeState > 0) {
                    document.body.classList.add(`text-size-${textSizeState}`);
                }

                // Update indicator bars
                const indicator = btn.querySelector('.text-size-indicator');
                if (indicator) {
                    const spans = indicator.querySelectorAll('span');
                    spans.forEach((span, index) => {
                        span.classList.toggle('active', index < textSizeState);
                    });
                }

                // Update button active state
                btn.classList.toggle('active', textSizeState > 0);
                break;

            case 'hideImages':
                document.body.classList.toggle('hide-images');
                break;

            case 'pauseAnimations':
                document.body.classList.toggle('no-animations');
                break;

            case 'tooltips':
                toggleTooltips();
                //main.classList.toggle('active');
                btn.classList.toggle('active');
                break;

            case 'highlightLinks':
                document.body.classList.toggle('highlight-links');
                break;
        }
    });
});

// Tooltip logic
let tooltipEnabled = false;
let currentTooltip = null;

function toggleTooltips() {
    tooltipEnabled = !tooltipEnabled;
    const elements = document.querySelectorAll('[data-tooltip]');

    if (tooltipEnabled) {
        // Create single tooltip element
        currentTooltip = document.createElement('div');
        currentTooltip.className = 'custom-tooltip';
        document.body.appendChild(currentTooltip);

        // Add event listeners to all elements
        elements.forEach(el => {
            el.addEventListener('mouseenter', showTooltip);
            el.addEventListener('mousemove', moveTooltip);
            el.addEventListener('mouseleave', hideTooltip);
        });
    } else {
        // Remove tooltip and clean up
        if (currentTooltip) {
            currentTooltip.remove();
            currentTooltip = null;
        }

        elements.forEach(el => {
            el.removeEventListener('mouseenter', showTooltip);
            el.removeEventListener('mousemove', moveTooltip);
            el.removeEventListener('mouseleave', hideTooltip);
        });
    }
}

function showTooltip(e) {
    if (!currentTooltip) return;
    currentTooltip.textContent = this.dataset.tooltip;
    currentTooltip.style.display = 'block';
    moveTooltip(e);
}

function moveTooltip(e) {
    if (!currentTooltip) return;
    const x = e.clientX + 15;
    const y = e.clientY + 15;
    currentTooltip.style.left = `${x}px`;
    currentTooltip.style.top = `${y}px`;
}

function hideTooltip() {
    if (currentTooltip) {
        currentTooltip.style.display = 'none';
    }
}




// — Form Validation —
(() => {
    'use strict';
    const form = document.getElementById('contactForm');
    form.addEventListener('submit', event => {
        if (!form.checkValidity()) {
            event.preventDefault(); event.stopPropagation();
        } else {
            event.preventDefault();
            alert('Thank you! Your message has been sent.');
            form.reset();
            form.classList.remove('was-validated');
        }
        form.classList.add('was-validated');
    }, false);
})();


